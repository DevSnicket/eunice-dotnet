module rec DevSnicket.Eunice.Analysis.Files.DependsUponReferences

open DevSnicket.Eunice.Analysis.Files
open DevSnicket.Eunice.Analysis.Files.Namespaces
open System

type ReferencesAndReferrer =
    {
        References: MethodOrTypeReference seq
        ReferrerType: Mono.Cecil.TypeReference
    }

let createDependsUponFromReferences (referencesAndReferrer: ReferencesAndReferrer): DependUpon list =
    referencesAndReferrer.References
    |> Seq.collect splitGenericArgumentsFromReference
    |> Seq.groupBy getTypeNamespaceAndNameFromReference
    |> Seq.collect (createItemAndNamespaceFromTypeAndMethodsOfReferrerType referencesAndReferrer.ReferrerType)
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
                yield! genericType.GenericArguments |> Seq.map TypeReference
            ]
        | _ ->
            seq [ reference ]

type private NameAndNamespace =
    {
        Name: string
        Namespace: string
    }

let private getTypeNamespaceAndNameFromReference reference =
    match reference with
    | MethodReference method -> method.DeclaringType
    | TypeReference ``type`` -> ``type``
    |> fun ``type`` ->
        {
            Name = ``type``.Name
            Namespace = ``type``.Namespace
        }

let private createItemAndNamespaceFromTypeAndMethodsOfReferrerType referrerType (``type``, references) =
    if ``type``.Namespace = "System" then
        seq []
    else if ``type``.Namespace = referrerType.Namespace && ``type``.Name = referrerType.Name then
        references
        |> Seq.choose createItemFromMethodReference
        |> Seq.map (fun methodReference -> { Item = methodReference; Namespace = "" })
    else
        seq [ {
            Item =
                {
                    Identifier =
                        ``type``.Name;
                    Items =
                        references
                        |> Seq.choose createItemFromMethodReference
                        |> Seq.toList
                }
            Namespace =
                ``type``.Namespace
        } ]

let private createItemFromMethodReference reference =
    match reference with
    | MethodReference method ->
        Some {
            Identifier = method.Name
            Items = []
        }
    | _ ->
        None

let private createDependUponFromNamespaceItem namespaceItem =
    {
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }