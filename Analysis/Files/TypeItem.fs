module DevSnicket.Eunice.Analysis.Files.TypeItem

let rec createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        createItemFromEnumOrInterfaceOrClass ``type``
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        createItemFromDelegate ``type``
    | _ ->
        createItemFromEnumOrInterfaceOrClass ``type``

and private createItemFromEnumOrInterfaceOrClass enumOrInterfaceOrClass =
    {
        Identifier =
            enumOrInterfaceOrClass.Name
        Items =
            [
                yield! enumOrInterfaceOrClass.NestedTypes |> Seq.map createItemFromType 
                yield! createItemsFromMethods enumOrInterfaceOrClass.Methods
            ]
    }

and private createItemFromDelegate ``delegate`` =
    {
        Identifier = ``delegate``.Name
        Items = []
    }

and private createItemsFromMethods methods =
    methods
    |> Seq.filter (fun method -> method.IsConstructor |> not)
    |> Seq.map createItemFromMethod

and private createItemFromMethod method =
    {
        Identifier = method.Name
        Items = []
    }
