module rec DevSnicket.Eunice.Analysis.Yaml.FormatItems

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.Yaml.FormatDependsUponMapping
open DevSnicket.Eunice.Analysis.Yaml.FormatKeyValueLinesMapping
open DevSnicket.Eunice.Analysis.Yaml.SequenceBlockEntryFromLines

let formatItems items =
    items
    |> Seq.collect (formatItem >> sequenceBlockEntryFromLines)

let private formatItem item =
    let mappingLines =
        [
            yield! formatDependsUponMapping item.DependsUpon
            yield! formatChildItemsMapping item.Items
        ]

    match mappingLines with
    | [] ->
        [ item.Identifier ]
    | _ ->
        [
            "id: " + item.Identifier
            yield! mappingLines
        ]

let private formatChildItemsMapping itemOrItems =
    let valueLines =
        match itemOrItems with
        | [] -> []
        | [ item ] -> formatItem item
        | _ -> formatItems itemOrItems |> Seq.toList

    formatKeyValueLinesMapping ("items", valueLines)