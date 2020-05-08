module DevSnicket.Eunice.Analysis.Files.Namespaces.NamespaceHierarchy

open DevSnicket.Eunice.Analysis.Files.Namespaces.Segments

let private segmentNamespace (itemAndNamespace: ItemAndNamespace<'T>) =
    {
        Item = itemAndNamespace.Item
        NamespaceSegments = itemAndNamespace.Namespace.Split "." |> Array.toList
    }

let groupNamespaceSegments createNamespaceItem =
    let rec groupNamespaceSegments itemsAndNamespaceSegments =
        itemsAndNamespaceSegments
        |> BaseNamespace.groupItemsByBaseNamespace
        |> Seq.collect createNamespaceOrGetItems
        |> Seq.toList

    and createNamespaceOrGetItems (``namespace``, itemsAndNamespaceSegments) =
        match ``namespace`` with
        | "" ->
            itemsAndNamespaceSegments
            |> Seq.map(fun itemAndNamespaceSegments -> itemAndNamespaceSegments.Item)
        | _ ->
            seq [
                createNamespaceItem({
                    Identifier = ``namespace``
                    Items = groupNamespaceSegments itemsAndNamespaceSegments
                })
            ]

    groupNamespaceSegments

let groupNamespaces createNamespaceItem items =
    items
    |> Seq.map segmentNamespace
    |> groupNamespaceSegments createNamespaceItem