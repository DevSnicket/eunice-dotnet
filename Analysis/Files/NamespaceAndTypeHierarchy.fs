module DevSnicket.Eunice.Analysis.Files.NamespaceAndTypeHierarchy

let private getItemsFromTypes =
    Seq.map(fun ``type`` -> ``type``.Item)

let rec groupTypesAndNamespaceSegments types =
    types
    |> BaseNamespace.groupTypes
    |> Seq.collect groupTypesInNamespace
    |> Seq.toList

and private groupTypesInNamespace (``namespace``, types) =
    let createItemForNamespace() =
        seq [ {
            Identifier = ``namespace``
            Items = groupTypesAndNamespaceSegments types
        } ]

    match ``namespace`` with
    | "" -> types |> getItemsFromTypes
    | _ -> createItemForNamespace()