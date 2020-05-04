module DevSnicket.Eunice.Analysis.Files.Tests.TestCases.ProjectFilePaths

open System.IO

let private getWhenProjectDirectoryPath directoryPath =
 let hasProjectFile =
  Path.Join(directoryPath, "TestCase.csproj")
  |> File.Exists
 
 match hasProjectFile with
 | true -> seq [ directoryPath ]
 | false -> seq []

let rec findInDirectoryPath directoryPath =
 directoryPath
 |> Directory.EnumerateDirectories
 |> Seq.collect getForSubdirectoryPath

and private getForSubdirectoryPath subdirectoryPath =
 seq [
  yield! getWhenProjectDirectoryPath subdirectoryPath
  yield! findInDirectoryPath(subdirectoryPath)
 ]