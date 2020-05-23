module DevSnicket.Eunice.Analysis.Files.Program
open System

[<EntryPoint>]
let main argv =
    async {
        match argv with
        | [| filePath |] ->
            let! yaml = filePath |> AssemblyAnalysis.analyzeAssemblyWithFilePath
            yaml |> Seq.iter Console.WriteLine
            return 0
        | _ ->
            return 1
    }
    |> Async.RunSynchronously