namespace DevSnicket.Eunice.Analysis.Files

open System

type DependUpon =
    {
        Identifier: String
        Items: DependUpon list
    }

type Item =
    {
        DependsUpon: DependUpon list
        Identifier: String
        Items: Item list
    }