namespace DevSnicket.Eunice.Analysis.Namespaces

open System

type ItemAndNamespace<'Item> =
    {
        Item: 'Item
        Namespace: String
    }