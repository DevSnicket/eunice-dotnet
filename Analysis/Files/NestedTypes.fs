module DevSnicket.Eunice.Analysis.Files.NestedTypes

let rec createIdentifierOrItemForType (``type``: Mono.Cecil.TypeDefinition) =
    let identifier = ``type``.Name

    match ``type``.NestedTypes with
    | nestedTypes when nestedTypes.Count = 0 ->
        Identifier(identifier)
    | nestedTypes ->
        Item({
            Identifier = identifier
            Items = nestedTypes |> createIdentifiersAndItemsForTypes
        })

and private createIdentifiersAndItemsForTypes =
    Seq.map createIdentifierOrItemForType
    >> Seq.toList