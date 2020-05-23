namespace DevSnicket.Eunice.Analysis

open System

type Item =
    {
        DependsUpon: DependUpon list
        Identifier: String
        Items: Item list
    }