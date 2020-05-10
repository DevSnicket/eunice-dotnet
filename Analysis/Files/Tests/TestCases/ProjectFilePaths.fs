module rec DevSnicket.Eunice.Analysis.Files.Tests.TestCases.ProjectFilePaths

open System.IO

let findInDirectoryPath directoryPath =
    directoryPath
    |> Directory.EnumerateDirectories
    |> Seq.collect getForSubdirectoryPath

let private getForSubdirectoryPath subdirectoryPath =
    seq [
        yield! getWhenProjectDirectoryPath subdirectoryPath
        yield! findInDirectoryPath(subdirectoryPath)
    ]

let private getWhenProjectDirectoryPath directoryPath =
    let hasProjectFile =
        Path.Join(directoryPath, "TestCase.csproj")
        |> File.Exists

    match hasProjectFile with
    | true -> seq [ directoryPath ]
    | false -> seq []