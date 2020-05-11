module rec DevSnicket.Eunice.Analysis.Files.DependsUponTypes

open DevSnicket.Eunice.Analysis.Files.Namespaces

let createDependsUponFromTypes (types: Mono.Cecil.TypeReference seq): DependUpon list =
    types
    |> Seq.collect createItemsAndNamespacesFromTypeAndGenericArguments
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

let private createItemsAndNamespacesFromTypeAndGenericArguments ``type`` =
    match ``type`` with
    | :? Mono.Cecil.GenericInstanceType as genericType ->
        seq [
            ``type``;
            yield! genericType.GenericArguments
        ]
        |> Seq.map createItemAndNamespaceFromType
    | _ ->
        seq [ ``type`` |> createItemAndNamespaceFromType ]

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