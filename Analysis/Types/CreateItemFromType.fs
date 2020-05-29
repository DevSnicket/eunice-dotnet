module rec DevSnicket.Eunice.Analysis.Types.CreateItemFromType

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.CreateDependsUponFromReferencesAndReferrer
open DevSnicket.Eunice.Analysis.CreateItemFromDelegate
open DevSnicket.Eunice.Analysis.GetReferencesInMethod
open DevSnicket.Eunice.Analysis.GetReferencesInMethodDeclaration
open DevSnicket.Eunice.Analysis.Types.GetReferencesFromConstructor
open DevSnicket.Eunice.Analysis.Types.GetTypesFromTypeDeclaration
open DevSnicket.Eunice.Analysis.Types.GetReferencesWhenYieldMethod
open DevSnicket.Eunice.Analysis.Types.NestedTypesAndYields.SeparateYieldsFromNestedTypes

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        ``type`` |> createItemFromClassOrInterface
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        ``type`` |> createItemFromDelegate
    | baseType when baseType.FullName = "System.Enum" ->
        {
            DependsUpon = []
            Identifier = ``type``.Name
            Items = []
        }
    | _ ->
        ``type`` |> createItemFromClassOrInterface

let private createItemFromClassOrInterface ``type`` =
    let rec createItemFromClassOrInterface () =
        {
            DependsUpon =
                ``type``
                |> getTypesFromTypeDeclaration
                |> createDependsUponFromTypes
            Identifier =
                ``type``.Name
            Items =
                createChildItems ()
        }

    and createChildItems () =
        let
            {
                NestedTypes = nestedTypes
                YieldReferencesByTypeName = yieldReferencesByTypeName
            }
            =
            ``type``.NestedTypes
            |> separateYieldsFromNestedTypes

        [
            yield! ``type``.Events |> Seq.map createItemFromEvent
            yield! ``type``.Fields |> Seq.choose createItemFromField
            yield! nestedTypes |> Seq.map createItemFromType
            yield! createItemsFromMethodsOrYields yieldReferencesByTypeName
            yield! ``type``.Properties |> Seq.map createItemFromProperty
        ]

    and createItemFromEvent event =
        {
            DependsUpon =
                [ event.EventType ]
                |> createDependsUponFromTypes
            Identifier =
                event.Name
            Items =
                []
        }

    and createItemFromField field =
        let rec createItemFromField () =
            match isEvent () || isProperty () with
            | true ->
                None
            | false ->
                Some {
                    DependsUpon =
                        [ field.FieldType ]
                        |> createDependsUponFromTypes
                    Identifier =
                        field.Name
                    Items =
                        []
                }

        and isEvent () =
            ``type``.Events
            |> Seq.exists (fun event -> event.Name = field.Name)

        and isProperty () =
            field.Name.StartsWith ("<Property>k__")

        createItemFromField ();

    and createItemsFromMethodsOrYields yieldReferencesByTypeName =
        let rec items () =
            ``type``.Methods
            |> Seq.choose createItemsFromMethodOrYield

        and createItemsFromMethodOrYield method =
            method |> createItemWhenYield
            |> Option.orElse (method |> createItemFromMethod)

        and createItemWhenYield method =
            let rec createItemWhenYield () =
                method
                |> getReferencesWhenYieldMethod yieldReferencesByTypeName
                |> Option.map createYieldItemWithReferences

            and createYieldItemWithReferences references =
                {
                    DependsUpon =
                        seq [
                            yield! method |> getReferencesInMethodDeclaration
                            yield! references
                        ]
                        |> createDependsUponFromReferences
                    Identifier =
                        method.Name
                    Items =
                        []
                }

            createItemWhenYield ()

        items ()

    and createItemFromMethod method =
        let isEvent = method.IsAddOn || method.IsRemoveOn
        let isProperty = method.IsGetter || method.IsSetter

        if isEvent || isProperty then
            None
        else if method.IsConstructor then
            method |> createItemFromConstructor
        else
            Some {
                DependsUpon =
                    method
                    |> getReferencesInMethod
                    |> createDependsUponFromReferences
                Identifier =
                    method.Name
                Items =
                    []
            }

    and createItemFromConstructor constructor =
        constructor
        |> getReferencesFromConstructor
        |> createDependsUponFromReferences
        |> function
            | [] ->
                None
            | dependsUpon ->
                Some {
                    DependsUpon = dependsUpon
                    Identifier = constructor.Name
                    Items = []
                }

    and createItemFromProperty property =
        {
            DependsUpon =
                [ property.PropertyType ]
                |> createDependsUponFromTypes
            Identifier =
                property.Name
            Items =
                []
        }

    and createDependsUponFromTypes types =
        types
        |> Seq.map TypeReference
        |> createDependsUponFromReferences

    and createDependsUponFromReferences references =
        createDependsUponFromReferencesAndReferrer
            {
                References = references
                ReferrerType = ``type``
            }

    createItemFromClassOrInterface ()