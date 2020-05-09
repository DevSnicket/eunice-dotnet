module DevSnicket.Eunice.Analysis.Files.DelegateItem

open DevSnicket.Eunice.Analysis.Files.Namespaces

let private createItemAndNamespaceFromType (``type``: Mono.Cecil.TypeReference): ItemAndNamespace<DependUpon> =
    {
        Item =
            {
                Identifier = ``type``.Name;
                Items = []
            }
        Namespace =
            ``type``.Namespace
    }

let private createDependUponFromNamespaceItem namespaceItem: DependUpon =
    {
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }

let createItemFromDelegate (``delegate``: Mono.Cecil.TypeDefinition) =
    let returnType =
        ``delegate``.Methods
        |> Seq.find (fun method -> method.Name = "Invoke")
        |> fun invoke -> invoke.ReturnType

    {
        DependsUpon =
            match returnType.FullName with
            | "System.Void" ->
                []
            | _ ->
                [ createItemAndNamespaceFromType returnType ]
                |> NamespaceHierarchy.groupNamespaces createDependUponFromNamespaceItem
        Identifier =
            ``delegate``.Name
        Items =
            []
    }
