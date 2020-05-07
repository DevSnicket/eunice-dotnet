module DevSnicket.Eunice.Analysis.Files.TypeIdentifierOrItem

let rec createIdentifierOrItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    let identifier = ``type``.Name

    match ``type`` |> createChildItemsOfType with
    | [] ->
        Identifier(identifier)
    | childItems ->
        Item({
            Identifier = identifier
            Items = childItems
        })

and private createChildItemsOfType ``type`` =
    seq [
        yield! ``type``.NestedTypes |> Seq.map createIdentifierOrItemFromType 
        yield! createFromMethodsOfType ``type``
    ]
    |> Seq.toList

and private createFromMethodsOfType ``type`` =
    match ``type``.BaseType with
    | null -> seq []
    | baseType when baseType.FullName = "System.MulticastDelegate" -> seq []
    | _ -> createIdentifiersFromMethods ``type``.Methods

and private createIdentifiersFromMethods =
    Seq.filter (fun method -> method.IsConstructor |> not)
    >> Seq.map (fun method -> Identifier(method.Name))