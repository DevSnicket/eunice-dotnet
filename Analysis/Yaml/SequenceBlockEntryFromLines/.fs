module DevSnicket.Eunice.Analysis.Yaml.SequenceBlockEntryFromLines

open DevSnicket.Eunice.Analysis.Yaml.IndentLines

let sequenceBlockEntryFromLines lines =
    match lines with
    | [] ->
        seq []
    | head :: tail ->
        seq [
            yield "- " + head
            yield! tail |> indentLines
        ]
