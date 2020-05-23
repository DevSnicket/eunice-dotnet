module rec DevSnicket.Eunice.Analysis.AnalyzePath

open DevSnicket.Eunice.Analysis.AnalyzeAssemblyPath
open System.IO

let analyzePath path =
    async {
        match path |> isDirectory with
        | true ->
            let! yamlForFiles =
                Directory.EnumerateFiles (path, "*.dll")
                |> Seq.sort
                |> Seq.map analyzeAssemblyPath
                |> Async.Parallel

            return yamlForFiles |> Seq.concat
        | false ->
            let! yaml = path |> analyzeAssemblyPath
            return yaml
    }

let private isDirectory =
    File.GetAttributes
    >> fun attributes -> attributes.HasFlag(FileAttributes.Directory)