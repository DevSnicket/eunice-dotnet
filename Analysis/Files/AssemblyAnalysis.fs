module rec DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open DevSnicket.Eunice.Analysis.Files.Namespaces
open Mono.Cecil
open System

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
    assemblyFilePath
    |> AssemblyDefinition.ReadAssembly
    |> fun assembly -> assembly.Modules
    |> Seq.collect createItemsForModule
    |> Yaml.Items.formatItems

let private createItemsForModule ``module`` =
    ``module``.Types
    |> Seq.filter (isModuleType >> not)
    |> Seq.map createItemForAndSegmentNamespaceOfType
    |> NamespaceHierarchy.groupNamespaces 
        {
            CreateNamespaceItem = createNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

let private createItemForAndSegmentNamespaceOfType ``type`` =
    {
        Item = TypeItems.createItemFromType ``type``
        Namespace = ``type``.Namespace
    }

let private createNamespaceItem namespaceItem =
    {
        DependsUpon = []
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }

let private isModuleType ``type`` =
    ``type``.Name = "<Module>"
    &&
    ``type``.Namespace = ""