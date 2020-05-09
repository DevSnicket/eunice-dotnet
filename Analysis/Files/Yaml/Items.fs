module DevSnicket.Eunice.Analysis.Files.Yaml.Items

open DevSnicket.Eunice.Analysis.Files

let rec private linesForItems items =
    items
    |> Seq.collect (linesForItem >> SequenceBlock.entryFromLines)

and private linesForItem item =
    let mappingLines =
        [
            yield! DependsUpon.linesForDependsUponMapping item.DependsUpon
            yield! linesForChildItemsMapping item.Items
        ]

    match mappingLines with
    | [] ->
        [ item.Identifier ]
    | _ ->
        [
            "id: " + item.Identifier
            yield! mappingLines
        ]

and private linesForChildItemsMapping itemOrItems =
    let valueLines =
        match itemOrItems with
        | [] -> []
        | [ item ] -> linesForItem item
        | _ -> linesForItems itemOrItems |> Seq.toList

    Mapping.keyValueLinesMapping ("items", valueLines)

let formatItems items =
    items
    |> linesForItems
    |> String.concat "\n"