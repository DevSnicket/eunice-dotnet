module DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open DevSnicket.Eunice.Analysis.Files.Namespaces
open Mono.Cecil
open System

let private isModuleType (``type``: TypeDefinition) =
    ``type``.Name = "<Module>"
    &&
    ``type``.Namespace = ""

let private createItemForAndSegmentNamespaceOfType (``type``: TypeDefinition) =
    {
        Item = TypeItem.createItemFromType ``type``
        Namespace = ``type``.Namespace
    }

let private createNamespaceItem namespaceItem =
    {
        DependsUpon = []
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }

let private createItemsForModule (``module``: ModuleDefinition) =
    ``module``.Types
    |> Seq.filter (isModuleType >> not)
    |> Seq.map createItemForAndSegmentNamespaceOfType
    |> NamespaceHierarchy.groupNamespaces 
        {
            CreateNamespaceItem = createNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
    assemblyFilePath
    |> AssemblyDefinition.ReadAssembly
    |> fun assembly -> assembly.Modules
    |> Seq.collect createItemsForModule
    |> Yaml.Items.formatItems