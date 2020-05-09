module DevSnicket.Eunice.Analysis.Files.Yaml.Mapping

let keyValueLinesMapping (key, valueLines) =
    let withoutBlock value =
        seq [ key + ": " + value ]

    let withBlock lines =
        seq [
            key + ":"
            yield! lines |> Indentation.indentLines
        ]

    match valueLines with
    | [] -> seq []
    | [ singleLine ] -> singleLine |> withoutBlock
    | _ -> valueLines |> withBlock
