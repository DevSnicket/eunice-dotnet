module DevSnicket.Eunice.Analysis.Files.Tests.TestsFromFileSystem

open System.IO

let testCases = TestCases.Parameters.createParameters

[<Xunit.Theory(DisplayName = "runTestsFromFileSystem")>]
[<Xunit.MemberData("testCases")>]
let runTestsFromFileSystem configuration directory =
    let loadExpected =
        Path.Join(directory, "Expected.yaml")
        |> File.ReadAllText

    let analyzeProject =
        Path.Join(directory, "bin", configuration, "TestCase.dll")
        |> DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis.analyzeAssemblyWithFilePath

    Xunit.Assert.Equal(
        loadExpected,
        analyzeProject
    )