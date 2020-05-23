module DevSnicket.Eunice.Analysis.Yaml.IndentLines

let indentLines lines =
    lines
    |> Seq.map (fun line -> "  " + line)