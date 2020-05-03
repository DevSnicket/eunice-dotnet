module DevSnicket.Eunice.Analysis.Files.Analyzer

open Mono.Cecil
open System

let private getYamlLinesFromTypes (``module``: ModuleDefinition) =
 ``module``.Types
 |> Seq.groupBy(fun ``type`` -> ``type``.Namespace)
 |> Seq.collect TypesByNamespace.GetYamlLines

let private getYamlLinesFromAssembly (assembly: AssemblyDefinition) =
 assembly.Modules
 |> Seq.collect getYamlLinesFromTypes
 |> String.concat "\n"

let Analyze (assemblyFilePath: String) =
 assemblyFilePath
 |> AssemblyDefinition.ReadAssembly
 |> getYamlLinesFromAssembly