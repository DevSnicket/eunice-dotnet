namespace DevSnicket.Eunice.Analysis.Files.Namespaces

open System

type NamespaceItem<'Item> =
    {
        Identifier: String
        Items: 'Item list
    }
