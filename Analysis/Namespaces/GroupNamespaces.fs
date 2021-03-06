module rec DevSnicket.Eunice.Analysis.Namespaces.GroupNamespaces

open DevSnicket.Eunice.Analysis.Namespaces.Segments
open DevSnicket.Eunice.Analysis.Namespaces.Segments.GroupItemsByBaseNamespace
open System

type Delegates<'Item> =
    {
        CreateNamespaceItem: NamespaceItem<'Item> -> 'Item
        GetIdentifierFromItem: 'Item -> String
    }

let groupNamespaces delegates (items: ItemAndNamespace<'T> seq) =
    items
    |> Seq.map segmentNamespace
    |> groupNamespaceSegments delegates

let private segmentNamespace itemAndNamespace =
    {
        Item = itemAndNamespace.Item
        NamespaceSegments = itemAndNamespace.Namespace.Split "." |> Array.toList
    }

let groupNamespaceSegments delegates =
    let rec groupNamespaceSegments itemsAndNamespaceSegments =
        itemsAndNamespaceSegments
        |> groupItemsByBaseNamespace
        |> Seq.collect createNamespaceOrGetItems
        |> Seq.sortBy delegates.GetIdentifierFromItem
        |> Seq.toList

    and createNamespaceOrGetItems (``namespace``, itemsAndNamespaceSegments) =
        match ``namespace`` with
        | "" ->
            itemsAndNamespaceSegments
            |> Seq.map (fun itemAndNamespaceSegments -> itemAndNamespaceSegments.Item)
        | _ ->
            seq [
                delegates.CreateNamespaceItem {
                    Identifier = ``namespace``
                    Items = groupNamespaceSegments itemsAndNamespaceSegments
                }
            ]

    groupNamespaceSegments