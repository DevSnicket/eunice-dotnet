module DevSnicket.Eunice.Analysis.Tests.TestCases.CreateParametersForTestCases

open DevSnicket.Eunice.Analysis.Tests.TestCases.AddConfigurationToProjectFilePaths
open DevSnicket.Eunice.Analysis.Tests.TestCases.FindTestCasesDirectoriesInPath
open System.IO

let createParametersForTestCases =
    Path.Join ("..", "..", "TestCases")
    |> findTestCasesDirectoriesInPath
    |> addConfigurationToProjectFilePaths
