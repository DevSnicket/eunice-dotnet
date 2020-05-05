module DevSnicket.Eunice.Analysis.Files.Yaml

let rec private linesFromIdentifiersAndItems identifiersAndItems =
    identifiersAndItems
    |> Seq.collect linesFromIdentifierOrItem

and private linesFromIdentifierOrItem identifierOrItem =
    match identifierOrItem with
    | Identifier identifier -> seq [ "- " + identifier ]
    | Item item -> linesFromItem item

and private linesFromItem item =
    seq [
        "- id: " + item.Identifier
        yield! linesFromItems item.Items
    ]

and private linesFromItems identifiersOrItems =
    let keyYaml = "  items:"

    let withoutBlock value =
        seq [ keyYaml + " " + value ]

    let withBlock lines =
        let linesWithIndent = lines |> Seq.map(fun line -> "    " + line)

        seq [ keyYaml; yield! linesWithIndent ]

    match identifiersOrItems with
    | [ Identifier singleIdentifier ] -> singleIdentifier |> withoutBlock
    | _ -> identifiersOrItems |> linesFromIdentifiersAndItems |> withBlock

let fromModel identifiersAndItems =
    identifiersAndItems
    |> linesFromIdentifiersAndItems
    |> String.concat "\n"