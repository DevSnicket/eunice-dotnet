module DevSnicket.Eunice.Analysis.Files.Yaml.SequenceBlock

let entryFromLines lines =
    match lines with
    | [] ->
        seq []
    | head :: tail ->
        seq [
            yield "- " + head
            yield! tail |> Indentation.indentLines
        ]
