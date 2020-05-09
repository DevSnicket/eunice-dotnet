module DevSnicket.Eunice.Analysis.Files.Yaml.Indentation

let indentLines lines =
    lines
    |> Seq.map (fun line -> "  " + line)