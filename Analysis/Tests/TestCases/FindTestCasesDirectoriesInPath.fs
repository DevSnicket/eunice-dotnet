module rec DevSnicket.Eunice.Analysis.Tests.TestCases.FindTestCasesDirectoriesInPath

open System.IO

let findTestCasesDirectoriesInPath directoryPath =
    directoryPath
    |> Directory.EnumerateDirectories
    |> Seq.collect getForSubdirectoryPath

let private getForSubdirectoryPath subdirectoryPath =
    seq [
        yield! subdirectoryPath |> getWhenProjectDirectoryPath
        yield! subdirectoryPath |> findTestCasesDirectoriesInPath
    ]

let private getWhenProjectDirectoryPath directoryPath =
    let hasProjectFile =
        Path.Join (directoryPath, "TestCase.csproj")
        |> File.Exists

    match hasProjectFile with
    | true -> seq [ directoryPath ]
    | false -> seq []