module rec DevSnicket.Eunice.Analysis.Files.Tests.TestsFromFileSystem

open System.IO

let testCases = TestCases.Parameters.createParameters

[<Xunit.Theory>]
[<Xunit.MemberData ("testCases")>]
let runTestsFromFileSystem configuration directory =
    async {
        let expectedPath =
            match Path.Join (directory, "Expected." + configuration + ".yaml") with
            | path when path |> File.Exists ->
                path
            | _ ->
                Path.Join (directory, "Expected.yaml")

        let! actual =
            Path.Join (directory, "bin", configuration, "TestCase.dll")
            |> DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis.analyzeAssemblyWithFilePath

        let expected =
            expectedPath |> File.ReadAllText

        if actual <> expected then
            raise (Xunit.Sdk.AssertActualExpectedException ("\n" + expected, "\n" + actual, directory))
    }