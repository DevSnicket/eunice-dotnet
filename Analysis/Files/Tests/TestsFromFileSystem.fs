module DevSnicket.Eunice.Analysis.Files.Tests.TestsFromFileSystem

open System.IO

let testCases = TestCases.Parameters.createParameters

[<Xunit.Theory(DisplayName = "runTestsFromFileSystem")>]
[<Xunit.MemberData("testCases")>]
let runTestsFromFileSystem configuration directory =
    let expectedPath =
        match Path.Join(directory, "Expected." + configuration + ".yaml") with
        | path when path |> File.Exists ->
            path
        | _ ->
            Path.Join(directory, "Expected.yaml")

    let actual =
        Path.Join(directory, "bin", configuration, "TestCase.dll")
        |> DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis.analyzeAssemblyWithFilePath

    Xunit.Assert.Equal(
        expectedPath |> File.ReadAllText,
        actual
    )