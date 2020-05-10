module rec DevSnicket.Eunice.Analysis.Files.Tests.TestCases.Configuration

open System;

let addConfigurationToProjectFilePaths projectFilesPaths =
    Seq.allPairs [ "Debug"; "Release" ] projectFilesPaths
    |> Seq.map getParameterArray

let private getParameterArray (configuration, directory) =
    [| configuration :> Object; directory :> Object |]