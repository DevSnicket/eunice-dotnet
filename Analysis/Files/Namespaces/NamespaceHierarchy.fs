module DevSnicket.Eunice.Analysis.Files.Namespaces.NamespaceHierarchy

open DevSnicket.Eunice.Analysis.Files.Namespaces.Segments
open System

type Delegates<'Item> =
    {
        CreateNamespaceItem: NamespaceItem<'Item> -> 'Item
        GetIdentifierFromItem: 'Item -> String
    }

let groupNamespaceSegments delegates =
    let rec groupNamespaceSegments itemsAndNamespaceSegments =
        itemsAndNamespaceSegments
        |> BaseNamespace.groupItemsByBaseNamespace
        |> Seq.collect createNamespaceOrGetItems
        |> Seq.sortBy delegates.GetIdentifierFromItem
        |> Seq.toList

    and createNamespaceOrGetItems (``namespace``, itemsAndNamespaceSegments) =
        match ``namespace`` with
        | "" ->
            itemsAndNamespaceSegments
            |> Seq.map(fun itemAndNamespaceSegments -> itemAndNamespaceSegments.Item)
        | _ ->
            seq [
                delegates.CreateNamespaceItem({
                    Identifier = ``namespace``
                    Items = groupNamespaceSegments itemsAndNamespaceSegments
                })
            ]

    groupNamespaceSegments

let private segmentNamespace (itemAndNamespace: ItemAndNamespace<'T>) =
    {
        Item = itemAndNamespace.Item
        NamespaceSegments = itemAndNamespace.Namespace.Split "." |> Array.toList
    }

let groupNamespaces delegates items =
    items
    |> Seq.map segmentNamespace
    |> groupNamespaceSegments delegates