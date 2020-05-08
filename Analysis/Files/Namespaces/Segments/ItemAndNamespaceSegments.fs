namespace DevSnicket.Eunice.Analysis.Files.Namespaces.Segments

open System

type ItemAndNamespaceSegments<'Item> =
    {
        Item: 'Item
        NamespaceSegments: String list
    }