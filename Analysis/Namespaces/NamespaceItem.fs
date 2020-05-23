namespace DevSnicket.Eunice.Analysis.Namespaces

open System

type NamespaceItem<'Item> =
    {
        Identifier: String
        Items: 'Item list
    }
