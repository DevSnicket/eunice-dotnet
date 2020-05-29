module rec DevSnicket.Eunice.Analysis.AnalyzeAssemblyPath

open DevSnicket.Eunice.Analysis.Namespaces
open DevSnicket.Eunice.Analysis.Namespaces.GroupNamespaces
open DevSnicket.Eunice.Analysis.Types.CreateItemFromType
open DevSnicket.Eunice.Analysis.Yaml.FormatItems
open System
open System.IO

let analyzeAssemblyPath (assemblyFilePath: String) =
    async {
        let! modules = assemblyFilePath |> readModulesFromAssemblyFilePath

        return
            modules
            |> Seq.collect createItemsForModule
            |> formatItems
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
    |> groupNamespaces
        {
            CreateNamespaceItem = createNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

let private createItemForAndSegmentNamespaceOfType ``type`` =
    {
        Item = createItemFromType ``type``
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