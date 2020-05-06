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
let rec linesForChildItems identifierOrItemOrIdentifiersAndItems =
    match identifierOrItemOrIdentifiersAndItems with
    | [] -> seq []
    | [ identifierOrItem ] -> linesForIdentifierOrItem identifierOrItem
    | _ -> linesForIdentifiersAndItems identifierOrItemOrIdentifiersAndItems
 
and private linesForIdentifiersAndItems (identifiersAndItems: IdentifierOrItem seq) =
    identifiersAndItems
    |> Seq.collect (linesForIdentifierOrItem >> Seq.toList >> blockSequenceLines)

and private linesForIdentifierOrItem identifierOrItem =
    match identifierOrItem with
    | Identifier identifier -> seq [ identifier ]
    | Item item -> linesForItem item

and private linesForItem item =
    seq [
        "id: " + item.Identifier
        yield! linesForChildItemsMapping item.Items
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

    match identifiersOrItems with
    | [] -> seq []
    | [ Identifier singleIdentifier ] -> singleIdentifier |> withoutBlock
    | _ -> identifiersOrItems |> linesForChildItems |> withBlock

let createForIdentifiersAndItems identifiersAndItems =
    identifiersAndItems
    |> linesForIdentifiersAndItems
    |> String.concat "\n"