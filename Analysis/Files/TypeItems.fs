module rec DevSnicket.Eunice.Analysis.Files.TypeItems

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        ``type`` |> createItemFromClassOrInterface
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        ``type`` |> Delegates.Item.createItemFromDelegate
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
                createDependsUpon ()
            Identifier =
                ``type``.Name
            Items =
                createChildItems ()
        }

    and createDependsUpon () =
        seq [
            if isNull ``type``.BaseType |> not then
                ``type``.BaseType
            yield! ``type``.GenericParameters |> getTypesOfGenericParameters
            yield! ``type``.Interfaces |> getTypesOfInterfaces
        ]
        |> createDependsUponFromTypes

    and createChildItems () =
        [
            yield! ``type``.Events |> Seq.map createItemFromEvent
            yield! ``type``.Fields |> createItemsFromFieldsExceptEvents ``type``.Events
            yield! ``type``.NestedTypes |> Seq.map createItemFromType
            yield! ``type``.Methods |> Seq.choose createItemFromMethod
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

    and createItemsFromFieldsExceptEvents events =
        Seq.filter (fun field -> events |> Seq.forall (fun event -> event.Name <> field.Name))
        >> Seq.choose createItemFromField

    and createItemFromField field =
        match field.Name.StartsWith ("<Property>k__") with
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
                    |> Methods.References.getReferencesOfMethod
                    |> createDependsUponFromReferences
                Identifier =
                    method.Name
                Items =
                    []
            }

    and createItemFromConstructor constructor =
        let dependsUpon =
            constructor
            |> Methods.References.getReferencesOfMethod
            |> Seq.filter (isReferenceToParameterlessConstructor >> not)
            |> createDependsUponFromReferences
        
        match dependsUpon with
        | [] ->
            None
        | _ ->
            Some {
                DependsUpon = dependsUpon
                Identifier = constructor.Name
                Items = []
            }

    and isReferenceToParameterlessConstructor reference =
        match reference with
        | MethodReference method ->
            method.Name = ".ctor" && method.Parameters.Count = 0
        | TypeReference _ ->
            false

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

    and createDependsUponFromReferences references =
        DependsUponReferences.createDependsUponFromReferences
            {
                References = references
                ReferrerType = ``type``
            }

    and createDependsUponFromTypes =
        Seq.map TypeReference
        >> createDependsUponFromReferences

    createItemFromClassOrInterface ()

let private getTypesOfGenericParameters parameters =
    parameters
    |> Seq.collect (fun parameter -> parameter.Constraints |> getTypesOfGenericConstraints)

let private getTypesOfGenericConstraints =
    Seq.map (fun ``constraint`` -> ``constraint``.ConstraintType)

let private getTypesOfInterfaces =
    Seq.map (fun ``interface`` -> ``interface``.InterfaceType)