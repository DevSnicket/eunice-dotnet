module rec DevSnicket.Eunice.Analysis.Files.TypeItem

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        createItemFromEnumOrInterfaceOrClass ``type``
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        Delegates.Item.createItemFromDelegate ``type``
    | _ ->
        createItemFromEnumOrInterfaceOrClass ``type``

let private createItemFromEnumOrInterfaceOrClass enumOrInterfaceOrClass =
    {
        DependsUpon =
            []
        Identifier =
            enumOrInterfaceOrClass.Name
        Items =
            [
                yield! enumOrInterfaceOrClass.NestedTypes |> Seq.map createItemFromType
                yield! createItemsFromMethods enumOrInterfaceOrClass.Methods
            ]
    }

let private createItemsFromMethods methods =
    methods
    |> Seq.filter (fun method -> method.IsConstructor |> not)
    |> Seq.map createItemFromMethod

let private createItemFromMethod method =
    {
        DependsUpon = []
        Identifier = method.Name
        Items = []
    }