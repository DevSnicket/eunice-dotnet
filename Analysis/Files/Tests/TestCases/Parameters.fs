module DevSnicket.Eunice.Analysis.Files.Tests.TestCases.Parameters

open System.IO

let createParameters =
    Path.Join ("..", "..", "TestCases")
    |> ProjectFilePaths.findInDirectoryPath
    |> Configuration.addConfigurationToProjectFilePaths
