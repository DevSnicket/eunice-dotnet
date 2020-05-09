module rec DevSnicket.Eunice.Analysis.Files.Delegates.DependsUponTypes

open DevSnicket.Eunice.Analysis.Files
open DevSnicket.Eunice.Analysis.Files.Namespaces

let createDependsUponFromTypes (types: Mono.Cecil.TypeReference seq): DependUpon list =
    types
    |> Seq.map createItemAndNamespaceFromType
    |> NamespaceHierarchy.groupNamespaces createDependUponFromNamespaceItem

let private createDependUponFromNamespaceItem namespaceItem =
    {
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }

let private createItemAndNamespaceFromType ``type`` =
    {
        Item =
            {
                Identifier = ``type``.Name;
                Items = []
            }
        Namespace =
            ``type``.Namespace
    }