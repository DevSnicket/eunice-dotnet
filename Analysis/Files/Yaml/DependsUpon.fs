module DevSnicket.Eunice.Analysis.Files.Yaml.DependsUpon

open DevSnicket.Eunice.Analysis.Files

let rec private linesForDependsUpon dependsUpon =
    dependsUpon
    |> Seq.collect linesForDependUpon
    |> Seq.toList

and private linesForDependUpon (dependUpon: DependUpon) =
    match dependUpon.Items with
    | [] ->
        seq [ dependUpon.Identifier ]
    | items ->
        seq [
            "id: " + dependUpon.Identifier
            yield! linesForDependUponItems items
        ]

and private linesForDependUponItems items =
    Mapping.keyValueLinesMapping (
        "items",
        linesForDependsUpon items
    )

let linesForDependsUponMapping dependsUpon =
    Mapping.keyValueLinesMapping (
        "dependsUpon",
        linesForDependsUpon dependsUpon
    )