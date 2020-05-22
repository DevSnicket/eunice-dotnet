module DevSnicket.Eunice.Analysis.Files.Program
open System

[<EntryPoint>]
let main argv =
    match argv with
    | [| filePath |] ->
        filePath |> AssemblyAnalysis.analyzeAssemblyWithFilePath |> Console.Write
        0
    | _ -> 1