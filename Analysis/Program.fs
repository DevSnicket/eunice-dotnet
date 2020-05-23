module DevSnicket.Eunice.Analysis.Program

open DevSnicket.Eunice.Analysis.AnalyzePath
open System

[<EntryPoint>]
let main arguments =
    match arguments with
    | [| path |] ->
        path
        |> analyzePath
        |> Async.RunSynchronously
        |> Seq.iter Console.WriteLine
        0
    | _ ->
        Console.WriteLine "Specify a file or directory path."
        1