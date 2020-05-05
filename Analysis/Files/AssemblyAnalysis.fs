module DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open Mono.Cecil
open System

let private segmentNamespaceOfType (``type``: TypeDefinition) =
 {
  IdentifierOrItem = NestedTypes.createIdentifierOrItemForType ``type``
  NamespaceSegments = ``type``.Namespace.Split "." |> Array.toList
 }

let private isModuleType (``type``: TypeDefinition) =
 ``type``.Name = "<Module>"
 &&
 ``type``.Namespace = ""

let private modelFromModule (``module``: ModuleDefinition) =
 ``module``.Types
 |> Seq.filter (isModuleType >> not)
 |> Seq.map segmentNamespaceOfType
 |> NamespaceAndTypeHierarchy.groupTypesAndNamespaceSegments

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
 assemblyFilePath
 |> AssemblyDefinition.ReadAssembly
 |> fun assembly -> assembly.Modules
 |> Seq.collect modelFromModule
 |> Yaml.fromModel