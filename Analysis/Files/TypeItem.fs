module rec DevSnicket.Eunice.Analysis.Files.TypeItem

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        {
            DependsUpon = []
            Identifier = ``type``.Name
            Items = createItemsFromType ``type``
        }
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        Delegates.Item.createItemFromDelegate ``type``
    | baseType ->
        {
            DependsUpon = createDependsUponFromBaseType baseType
            Identifier = ``type``.Name
            Items = createItemsFromType ``type``
        }

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

let private createDependsUponFromBaseType baseType =
    let isRelevant =
        [ "System.Enum"; "System.Object" ]
        |> List.contains baseType.FullName
        |> not

    match isRelevant with
    | false -> []
    | true -> DependsUponTypes.createDependsUponFromTypes [ baseType ]