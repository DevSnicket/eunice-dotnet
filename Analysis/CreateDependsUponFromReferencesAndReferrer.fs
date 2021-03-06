module rec DevSnicket.Eunice.Analysis.CreateDependsUponFromReferencesAndReferrer

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.Namespaces
open DevSnicket.Eunice.Analysis.Namespaces.GroupNamespaces

type ReferencesAndReferrer =
    {
        References: Reference seq
        ReferrerType: Mono.Cecil.TypeReference
    }

let createDependsUponFromReferencesAndReferrer (referencesAndReferrer: ReferencesAndReferrer): DependUpon list =
    referencesAndReferrer.References
    |> Seq.collect splitGenericArgumentsFromReference
    |> Seq.groupBy getTypeNamespaceAndNameFromReference
    |> Seq.collect (createItemAndNamespaceFromTypeAndMethodsOfReferrerType referencesAndReferrer.ReferrerType)
    |> groupNamespaces
        {
            CreateNamespaceItem = createDependUponFromNamespaceItem
            GetIdentifierFromItem = fun item -> item.Identifier
        }

let private splitGenericArgumentsFromReference reference =
    match reference with
    | FieldReference _
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
    | FieldReference field ->
        field.DeclaringType
    | MethodReference method ->
        method.DeclaringType
    | TypeReference ``type`` ->
        match ``type``.DeclaringType with
        | null -> ``type``
        | declaringType -> declaringType
    |> fun ``type`` ->
        {
            Name = ``type``.Name
            Namespace = ``type``.Namespace
        }

let private createItemAndNamespaceFromTypeAndMethodsOfReferrerType referrerType (itemType, references) =
    let isTypeReferenceOfItemType (otherType: Mono.Cecil.TypeReference) =
        itemType.Namespace = otherType.Namespace
        &&
        itemType.Name = otherType.Name

    let createItemFromReference reference: DependUpon option =
        match reference with
        | FieldReference fieldReference ->
            Some {
                Identifier = fieldReference.Name
                Items = []
            }
        | MethodReference methodReference ->
            Some {
                Identifier = methodReference.Name
                Items = []
            }
        | TypeReference typeReference ->
            match typeReference |> isTypeReferenceOfItemType with
            | true ->
                None
            | false ->
                Some {
                    Identifier = typeReference.Name
                    Items = []
                }

    if itemType.Namespace = "System" then
        seq []
    else if referrerType |> isTypeReferenceOfItemType then
        references
        |> Seq.choose createItemFromReference
        |> Seq.map (fun item -> { Item = item; Namespace = "" })
    else
        seq [ {
            Item =
                {
                    Identifier =
                        itemType.Name;
                    Items =
                        references
                        |> Seq.choose createItemFromReference
                        |> Seq.toList
                }
            Namespace =
                itemType.Namespace
        } ]

let private createDependUponFromNamespaceItem namespaceItem =
    {
        Identifier = namespaceItem.Identifier
        Items = namespaceItem.Items
    }