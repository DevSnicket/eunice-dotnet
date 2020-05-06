module DevSnicket.Eunice.Analysis.Files.NamespaceAndTypeHierarchy

let private getIdentifiersAndItemsFromTypes =
    Seq.map(fun ``type`` -> ``type``.IdentifierOrItem)

let rec groupTypesAndNamespaceSegments types =
    types
    |> BaseNamespace.groupTypes
    |> Seq.collect groupTypesInNamespace
    |> Seq.toList

and private groupTypesInNamespace (``namespace``, types) =
    let createItemForNamespace() =
        seq [ Item({
            Identifier = ``namespace``
            Items = groupTypesAndNamespaceSegments types
        }) ]

    match ``namespace`` with
    | "" -> types |> getIdentifiersAndItemsFromTypes
    | _ -> createItemForNamespace()