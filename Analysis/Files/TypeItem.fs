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
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        Delegates.Item.createItemFromDelegate ``type``
    | baseType ->
        {
            DependsUpon =
                seq [
                    baseType;
                    yield! ``type``.Interfaces |> getTypesOfInterfaces
                ]
                |> createDependsUponFromReferencedTypes
            Identifier =
                ``type``.Name
            Items =
                ``type`` |> createItemsFromType
        }

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
        yield! ``type``.NestedTypes |> Seq.map createItemFromType
        yield! createItemsFromMethods ``type``.Methods
    ]

let private createItemsFromMethods methods =
    methods
    |> Seq.filter (fun method -> method.IsConstructor |> not)
    |> Seq.map createItemFromMethod

let private createItemFromMethod method =
    {
        DependsUpon = Methods.DependsUpon.createDependsUponFromMethod method
        Identifier = method.Name
        Items = []
    }