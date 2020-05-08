namespace DevSnicket.Eunice.Analysis.Files

open System

type Item =
    {
        Identifier: String
        Items: Item list
    }