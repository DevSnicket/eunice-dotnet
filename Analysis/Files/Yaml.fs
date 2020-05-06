module DevSnicket.Eunice.Analysis.Files.Yaml

let private indentLines lines =
    lines
    |> Seq.map (fun line -> "  " + line)

let private blockSequenceLines lines =
    match lines with
    | [] -> seq []
    | head :: tail ->
        seq [
            yield "- " + head
            yield! tail |> indentLines
        ]

let rec private linesForChildItems identifierOrItemOrIdentifiersAndItems =
    match identifierOrItemOrIdentifiersAndItems with
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

and private linesForChildItemsMapping identifiersOrItems =
    let keyYaml = "items:"

    let withoutBlock value =
        seq [ keyYaml + " " + value ]

    let withBlock lines =
        seq [
            keyYaml
            yield! lines |> indentLines
        ]

    match identifiersOrItems with
    | [ Identifier singleIdentifier ] -> singleIdentifier |> withoutBlock
    | _ -> identifiersOrItems |> linesForChildItems |> withBlock

let createForIdentifiersAndItems identifiersAndItems =
    identifiersAndItems
    |> linesForIdentifiersAndItems
    |> String.concat "\n"