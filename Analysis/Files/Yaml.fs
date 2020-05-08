module DevSnicket.Eunice.Analysis.Files.Yaml

open System

let private indentLines lines =
    lines
    |> Seq.map (fun line -> "  " + line)

// public so the empty can be tested
let blockSequenceLines lines =
    match lines with
    | [] -> seq []
    | head :: tail ->
        seq [
            yield "- " + head
            yield! tail |> indentLines
        ]

let private createKeyValueLinesMapping (key, valueLines) =
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

let rec private linesForChildItems itemOrItems =
    match itemOrItems with
    | [] -> []
    | [ item ] -> linesForItem item
    | _ -> linesForItems itemOrItems |> Seq.toList
 
and private linesForItems (items: Item seq) =
    items
    |> Seq.collect (linesForItem >> blockSequenceLines)

and private linesForItem item =
    let mappingLines =
        [
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

and private linesForChildItemsMapping items =
    createKeyValueLinesMapping(
        "items",
        items |> linesForChildItems
    )

let createForItems items =
    items
    |> linesForItems
    |> String.concat "\n"