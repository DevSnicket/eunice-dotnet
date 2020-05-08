namespace DevSnicket.Eunice.Analysis.Files.Namespaces

open System

type ItemAndNamespace<'Item> =
    {
        Item: 'Item
        Namespace: String
    }