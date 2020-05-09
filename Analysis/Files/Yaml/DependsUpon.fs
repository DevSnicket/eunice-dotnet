module rec DevSnicket.Eunice.Analysis.Files.Yaml.DependsUpon

open DevSnicket.Eunice.Analysis.Files

let linesForDependsUponMapping (dependsUpon: DependUpon list) =
    Mapping.keyValueLinesMapping (
        "dependsUpon",
        linesForDependsUpon dependsUpon
    )

let private linesForDependsUpon dependsUpon =
    match dependsUpon with
    | [ singleDependUpon ] ->
        linesForDependUpon singleDependUpon
    | _ ->
        dependsUpon
        |> Seq.collect (linesForDependUpon >> SequenceBlock.entryFromLines)
        |> Seq.toList

let private linesForDependUpon dependUpon =
    match dependUpon.Items with
    | [] ->
        [ dependUpon.Identifier ]
    | items ->
        [
            "id: " + dependUpon.Identifier
            yield! linesForDependUponItemsMapping items
        ]

let private linesForDependUponItemsMapping items =
    Mapping.keyValueLinesMapping (
        "items",
        linesForDependsUpon items
    )