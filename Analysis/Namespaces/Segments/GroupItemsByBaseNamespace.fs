module rec DevSnicket.Eunice.Analysis.Namespaces.Segments.GroupItemsByBaseNamespace

open System

type private ItemAndBaseNamespace<'Item> =
    {
        BaseNamespace: String
        ItemAndNamespaceSegments: ItemAndNamespaceSegments<'Item>
    }

let groupItemsByBaseNamespace items =
    items
    |> Seq.map dequeueBaseNamespaceFromItem
    |> Seq.groupBy (fun item -> item.BaseNamespace)
    |> Seq.map (
        fun (baseNamespace, itemsAndBaseNamespaces) ->
            (
                baseNamespace,
                itemsAndBaseNamespaces |> Seq.map (fun itemAndBaseNamespace -> itemAndBaseNamespace.ItemAndNamespaceSegments)
            )
        )

let private dequeueBaseNamespaceFromItem itemAndNamespaceSegments =
    match itemAndNamespaceSegments.NamespaceSegments with
    | head :: tail ->
        {
            BaseNamespace = head
            ItemAndNamespaceSegments = { itemAndNamespaceSegments with NamespaceSegments = tail }
        }
    | [] ->
        {
            BaseNamespace = ""
            ItemAndNamespaceSegments = itemAndNamespaceSegments
        }