module DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open Mono.Cecil
open System

let private createItemForAndSegmentNamespaceOfType (``type``: TypeDefinition) =
    {
        Item = TypeItem.createItemFromType ``type``
        NamespaceSegments = ``type``.Namespace.Split "." |> Array.toList
    }

let private isModuleType (``type``: TypeDefinition) =
    ``type``.Name = "<Module>"
    &&
    ``type``.Namespace = ""

let private createItemsForModule (``module``: ModuleDefinition) =
    ``module``.Types
    |> Seq.filter (isModuleType >> not)
    |> Seq.map createItemForAndSegmentNamespaceOfType
    |> NamespaceAndTypeHierarchy.groupTypesAndNamespaceSegments

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
    assemblyFilePath
    |> AssemblyDefinition.ReadAssembly
    |> fun assembly -> assembly.Modules
    |> Seq.collect createItemsForModule
    |> Yaml.createForItems