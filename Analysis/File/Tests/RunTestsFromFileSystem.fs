module DevSnicket.Eunice.Analysis.File.Tests

open System.IO
open System

let TestCases =
 let directories =
  Path.Join("..", "..", "TestCases")
  |> Directory.EnumerateDirectories
 
 Seq.allPairs [ "Debug"; "Release" ] directories
 |> Seq.map(fun (configuration, directory) -> [| configuration :> Object; directory :> Object |])

[<Xunit.Theory>]
[<Xunit.MemberData("TestCases")>]
let RunTestsFromFileSystem configuration directory =
 let loadExpected =
  Path.Join(directory, "Expected.yaml")
  |> File.ReadAllText

 let analyzeProject =
  Path.Join(directory, "bin", configuration, "TestCase.dll")
  |> Analyze

 Xunit.Assert.Equal(
  loadExpected,
  analyzeProject
 )