module rec DevSnicket.Eunice.Analysis.Files.TypeItem

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        {
            DependsUpon =
                ``type``.Interfaces
                |> getTypesOfInterfaces
                |> createDependsUponFromReferencedTypes
            Identifier =
                ``type``.Name
            Items =
                ``type`` |> createItemsFromType
        }
    | baseType when baseType.FullName = "System.Enum" ->
        {
            DependsUpon = []
            Identifier = ``type``.Name
            Items = []
        }
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        Delegates.Item.createItemFromDelegate ``type``
    | baseType ->
        {
            DependsUpon =
                seq [
                    baseType;
                    yield! ``type``.GenericParameters |> getTypesOfGenericParameters
                    yield! ``type``.Interfaces |> getTypesOfInterfaces
                ]
                |> createDependsUponFromReferencedTypes
            Identifier =
                ``type``.Name
            Items =
                ``type`` |> createItemsFromType
        }

let private getTypesOfGenericParameters parameters =
    parameters
    |> Seq.collect (fun parameter -> parameter.Constraints |> getTypesOfGenericConstraints)

let private getTypesOfGenericConstraints =
    Seq.map (fun ``constraint`` -> ``constraint``.ConstraintType)

let private getTypesOfInterfaces =
    Seq.map (fun ``interface`` -> ``interface``.InterfaceType)

let private createDependsUponFromReferencedTypes types =
    types
    |> Seq.filter isDependUponTypeRelevant
    |> DependsUponTypes.createDependsUponFromTypes

let private isDependUponTypeRelevant ``type`` =
    [ "System.Enum"; "System.Object" ]
    |> List.contains ``type``.FullName
    |> not

let private createItemsFromType ``type`` =
    [
        yield! ``type``.Events |> Seq.map createItemFromEvent
        yield! ``type``.Fields |> Seq.map createItemFromField
        yield! ``type``.NestedTypes |> Seq.map createItemFromType
        yield! ``type``.Methods |> Seq.collect createItemsFromMethod
        yield! ``type``.Properties |> Seq.map createItemFromProperty
    ]

let private createItemFromEvent event =
    {
        DependsUpon =
            [ event.EventType ]
            |> DependsUponTypes.createDependsUponFromTypes
        Identifier =
            event.Name
        Items =
            []
    }

let private createItemFromField field =
    {
        DependsUpon =
            [ field.FieldType ]
            |> DependsUponTypes.createDependsUponFromTypes
        Identifier =
            field.Name
        Items =
            []
    }

let private createItemsFromMethod method =
    let isEvent = method.IsAddOn || method.IsRemoveOn
    let isProperty = method.IsGetter || method.IsSetter

    if isEvent || isProperty then
        seq []
    else
        let dependsUpon = method |> Methods.DependsUpon.createDependsUponFromMethod
        
        match (dependsUpon, method.IsConstructor) with
        | ([], true) ->
            seq []
        | _ ->
            seq [
                {
                    DependsUpon = dependsUpon
                    Identifier = method.Name
                    Items = []
                }
            ]

let private createItemFromProperty property =
    {
        DependsUpon =
            [ property.PropertyType ]
            |> DependsUponTypes.createDependsUponFromTypes
        Identifier =
            property.Name
        Items =
            []
    }