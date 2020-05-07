module DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open Mono.Cecil
open System

let private createIdentifierOrItemForAndSegmentNamespaceOfType (``type``: TypeDefinition) =
    {
        IdentifierOrItem = TypeIdentifierOrItem.createIdentifierOrItemFromType ``type``
        NamespaceSegments = ``type``.Namespace.Split "." |> Array.toList
    }

let private isModuleType (``type``: TypeDefinition) =
    ``type``.Name = "<Module>"
    &&
    ``type``.Namespace = ""

let private createIdentifiersAndItemsForModule (``module``: ModuleDefinition) =
    ``module``.Types
    |> Seq.filter (isModuleType >> not)
    |> Seq.map createIdentifierOrItemForAndSegmentNamespaceOfType
    |> NamespaceAndTypeHierarchy.groupTypesAndNamespaceSegments

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
    assemblyFilePath
    |> AssemblyDefinition.ReadAssembly
    |> fun assembly -> assembly.Modules
    |> Seq.collect createIdentifiersAndItemsForModule
    |> Yaml.createForIdentifiersAndItems