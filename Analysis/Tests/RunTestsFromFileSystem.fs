module rec DevSnicket.Eunice.Analysis.Tests.RunTestsFromFileSystem

open DevSnicket.Eunice.Analysis.AnalyzeAssemblyPath
open DevSnicket.Eunice.Analysis.Tests.TestCases.CreateParametersForTestCases
open System.IO

let parametersForTestCases = createParametersForTestCases

[<Xunit.Theory>]
[<Xunit.MemberData ("parametersForTestCases")>]
let runTestsFromFileSystem configuration directory =
    async {
        let expectedPath =
            match Path.Join (directory, "Expected." + configuration + ".yaml") with
            | path when path |> File.Exists ->
                path
            | _ ->
                Path.Join (directory, "Expected.yaml")

        let! actualLines =
            Path.Join (directory, "bin", configuration, "TestCase.dll")
            |> analyzeAssemblyPath

        let actual =
            actualLines
            |> String.concat "\n"

        let! expected =
            expectedPath
            |> File.ReadAllTextAsync
            |> Async.AwaitTask

        if actual <> expected then
            raise (Xunit.Sdk.AssertActualExpectedException ("\n" + expected, "\n" + actual, directory))
    }