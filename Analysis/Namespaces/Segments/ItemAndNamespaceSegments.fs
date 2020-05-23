namespace DevSnicket.Eunice.Analysis.Namespaces.Segments

open System

type ItemAndNamespaceSegments<'Item> =
    {
        Item: 'Item
        NamespaceSegments: String list
    }