module DevSnicket.Eunice.Analysis.Files.Tests.TestsFromFileSystem

open System.IO

let TestCases = TestCases.Parameters.createParameters

[<Xunit.Theory>]
[<Xunit.MemberData("TestCases")>]
let RunTestsFromFileSystem configuration directory =
 let loadExpected =
  Path.Join(directory, "Expected.yaml")
  |> File.ReadAllText

 let analyzeProject =
  Path.Join(directory, "bin", configuration, "TestCase.dll")
  |> DevSnicket.Eunice.Analysis.Files.Analyzer.Analyze

 Xunit.Assert.Equal(
  loadExpected,
  analyzeProject
 )