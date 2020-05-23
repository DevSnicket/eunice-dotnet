module rec DevSnicket.Eunice.Analysis.Files.AssemblyAnalysis

open DevSnicket.Eunice.Analysis.Files.Namespaces
open System
open System.IO

let analyzeAssemblyWithFilePath (assemblyFilePath: String) =
    async {
        let! modules = assemblyFilePath |> readModulesFromAssemblyFilePath

        return
            modules
            |> Seq.collect createItemsForModule
            |> Yaml.Items.formatItems
    }

let private readModulesFromAssemblyFilePath assemblyFilePath =
    async {
        let! buffer =
            File.ReadAllBytesAsync(assemblyFilePath)
            |> Async.AwaitTask

        return
            new MemoryStream(buffer, writable = false)
            |> Mono.Cecil.AssemblyDefinition.ReadAssembly
            |> fun assembly -> assembly.Modules
    }

let private createItemsForModule (``module``: Mono.Cecil.ModuleDefinition) =
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