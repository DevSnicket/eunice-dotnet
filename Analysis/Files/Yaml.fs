module DevSnicket.Eunice.Analysis.Files.Yaml

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

// public so the empty can be tested
let rec linesForChildItems itemOrItems =
    match itemOrItems with
    | [] -> seq []
    | [ item ] -> linesForItem item
    | _ -> linesForItems itemOrItems
 
and private linesForItems (identifiers: Item seq) =
    identifiers
    |> Seq.collect (linesForItem >> Seq.toList >> blockSequenceLines)

and private linesForItem item =
    match item.Items with
    | [] ->
        seq [ item.Identifier ]
    | items ->
        seq [
            "id: " + item.Identifier
            yield! linesForChildItemsMapping items
        ]

// public so the empty can be tested
and linesForChildItemsMapping identifiersOrItems =
    let keyYaml = "items:"

    let withoutBlock value =
        seq [ keyYaml + " " + value ]

    let withBlock lines =
        seq [
            keyYaml
            yield! lines |> indentLines
        ]

    match identifiersOrItems |> linesForChildItems |> Seq.toList with
    | [] -> seq []
    | [ singleLine ] -> singleLine |> withoutBlock
    | lines -> lines |> withBlock

let createForItems items =
    items
    |> linesForItems
    |> String.concat "\n"