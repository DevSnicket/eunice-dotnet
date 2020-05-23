module DevSnicket.Eunice.Analysis.Yaml.FormatKeyValueLinesMapping

open DevSnicket.Eunice.Analysis.Yaml.IndentLines

let formatKeyValueLinesMapping (key, valueLines) =
    let withoutBlock value =
        seq [ key + ": " + value ]

    let withBlock lines =
        seq [
            key + ":"
            yield! lines |> indentLines
        ]

    match valueLines with
    | [] -> seq []
    | [ singleLine ] -> singleLine |> withoutBlock
    | _ -> valueLines |> withBlock
