module rec DevSnicket.Eunice.Analysis.AnalyzePath.Test

open DevSnicket.Eunice.Analysis.AnalyzePath
open System
open System.IO

let parametersForTestCases () =
    [
        [|
            seq [ "Directory"; "bin" ]
            seq [ "- ClassInFile1"; "- ClassInFile2" ]
        |]
        [|
            seq [ "File"; "bin"; "File.dll" ]
            seq [ "- Class" ]
        |]
    ]

[<Xunit.Theory>]
[<Xunit.MemberData ("parametersForTestCases")>]
let runTestsFromFileSystem pathSegments expected =
    async {
        let! actual =
            [|
                ".."; ".."; ".."; "AnalyzePath"; "TestCases";
                yield! pathSegments
            |]
            |> Path.Join
            |> analyzePath

        Xunit.Assert.Equal<String seq>(
            expected,
            actual
        )
    }