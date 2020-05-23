namespace DevSnicket.Eunice.Analysis

open System

type DependUpon =
    {
        Identifier: String
        Items: DependUpon list
    }