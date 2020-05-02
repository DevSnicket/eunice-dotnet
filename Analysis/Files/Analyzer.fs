module DevSnicket.Eunice.Analysis.Files.Analyzer

open Mono.Cecil
open System

let GetYamlLinesFromType (``type``: TypeDefinition) =
 let withNamespace() =
  [
   "- id: " + ``type``.Namespace
   "  items: " + ``type``.Name
  ];

 match ``type`` with
 | ``type`` when ``type``.Namespace <> "" -> withNamespace()
 | ``type`` when ``type``.Name = "<Module>" -> []
 | _ -> [ "- " + ``type``.Name ]

let GetYamlLinesFromTypes (``module``: ModuleDefinition) =
 ``module``.Types
 |> Seq.collect GetYamlLinesFromType

let GetYamlLinesFromAssembly (assembly: AssemblyDefinition) =
 assembly.Modules
 |> Seq.collect(GetYamlLinesFromTypes)
 |> fun lines -> String.Join("\n", lines)

let Analyze (assemblyFilePath: String) =
 assemblyFilePath
 |> AssemblyDefinition.ReadAssembly
 |> GetYamlLinesFromAssembly