module rec DevSnicket.Eunice.Analysis.Files.DependsUponTypes

open DevSnicket.Eunice.Analysis.Files.Namespaces

let createDependsUponFromTypes (types: Mono.Cecil.TypeReference seq): DependUpon list =
    types
    |> Seq.map createItemAndNamespaceFromType
    |> NamespaceHierarchy.groupNamespaces
        {
            CreateNamespaceItem = createDependUponFromNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

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