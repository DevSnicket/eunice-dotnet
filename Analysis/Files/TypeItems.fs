module rec DevSnicket.Eunice.Analysis.Files.TypeItems

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        {
            DependsUpon =
                ``type``.Interfaces
                |> getTypesOfInterfaces
                |> createDependsUponFromTypes
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
                |> createDependsUponFromTypes
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

let private createItemsFromType ``type`` =
    [
        yield! ``type``.Events |> Seq.map createItemFromEvent
        yield! ``type``.Fields |> createItemsFromFieldsExceptEvents ``type``.Events
        yield! ``type``.NestedTypes |> Seq.map createItemFromType
        yield! ``type``.Methods |> Seq.collect createItemsFromMethod
        yield! ``type``.Properties |> Seq.map createItemFromProperty
    ]

let private createItemFromEvent event =
    {
        DependsUpon =
            [ event.EventType ]
            |> createDependsUponFromTypes
        Identifier =
            event.Name
        Items =
            []
    }

let private createItemsFromFieldsExceptEvents events =
    Seq.filter (fun field -> events |> Seq.forall (fun event -> event.Name <> field.Name))
    >> Seq.collect createItemsFromField

let private createItemsFromField field =
    match field.Name.StartsWith("<Property>k__") with
    | true ->
        seq []
    | false ->
        seq [ {
            DependsUpon =
                [ field.FieldType ]
                |> createDependsUponFromTypes
            Identifier =
                field.Name
            Items =
                []
        } ]

let private createItemsFromMethod method =
    let isEvent = method.IsAddOn || method.IsRemoveOn
    let isProperty = method.IsGetter || method.IsSetter

    if isEvent || isProperty then
        seq []
    else if method.IsConstructor then
        createItemsFromConstructor method
    else
        seq [ {
            DependsUpon =
                method
                |> Methods.References.getReferencesOfMethod
                |> DependsUponReferences.createDependsUponFromReferences
            Identifier =
                method.Name
            Items =
                []
        } ]

let private createItemsFromConstructor constructor =
    let dependsUpon =
        constructor
        |> Methods.References.getReferencesOfMethod
        |> Seq.filter (isReferenceToParameterlessConstructor >> not)
        |> DependsUponReferences.createDependsUponFromReferences
    
    match dependsUpon with
    | [] ->
        seq []
    | _ ->
        seq [ {
            DependsUpon = dependsUpon
            Identifier = constructor.Name
            Items = []
        } ]

let private isReferenceToParameterlessConstructor reference =
    match reference with
    | MethodReference method ->
        method.Name = ".ctor" && method.Parameters.Count = 0
    | TypeReference _ ->
        false

let private createItemFromProperty property =
    {
        DependsUpon =
            [ property.PropertyType ]
            |> createDependsUponFromTypes
        Identifier =
            property.Name
        Items =
            []
    }

let private createDependsUponFromTypes =
    Seq.map TypeReference
    >> DependsUponReferences.createDependsUponFromReferences