module rec DevSnicket.Eunice.Analysis.Files.DependsUponReferences

open DevSnicket.Eunice.Analysis.Files
open DevSnicket.Eunice.Analysis.Files.Namespaces

let createDependsUponFromReferences (references: MethodOrTypeReference seq): DependUpon list =
    references
    |> Seq.collect splitGenericArgumentsFromReference
    |> Seq.groupBy getTypeNamespaceAndNameFromReference
    |> Seq.collect createItemsAndNamespacesFromTypeAndMethods
    |> NamespaceHierarchy.groupNamespaces
        {
            CreateNamespaceItem = createDependUponFromNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

let private splitGenericArgumentsFromReference reference =
    match reference with
    | MethodReference _ ->
        seq [ reference ]
    | TypeReference ``type`` ->
        match ``type`` with
        | :? Mono.Cecil.GenericInstanceType as genericType ->
            seq [
                reference
                yield! genericType.GenericArguments |> Seq.map (fun argument -> TypeReference(argument))
            ]
        | _ ->
            seq [ reference ]

let private getTypeNamespaceAndNameFromReference reference =
    match reference with
    | MethodReference method -> method.DeclaringType |> getNamespaceAndNameFromType
    | TypeReference ``type`` -> ``type`` |> getNamespaceAndNameFromType

let private getNamespaceAndNameFromType ``type`` =
    {|
        Name = ``type``.Name
        Namespace = ``type``.Namespace
    |}

let private createItemsAndNamespacesFromTypeAndMethods (``type``, references) =
    match ``type``.Namespace with
    | "System" ->
        seq []
    | _ ->
        seq [ {
            Item =
                {
                    Identifier =
                        ``type``.Name;
                    Items =
                        references |> Seq.collect createItemsFromMethodReference |> Seq.toList
                }
            Namespace =
                ``type``.Namespace
        } ]

let private createItemsFromMethodReference reference =
    match reference with
    | MethodReference method -> seq [ { Identifier = method.Name; Items = [] } ]
    | _ -> seq []

let private createDependUponFromNamespaceItem namespaceItem =
    {
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }