module DevSnicket.Eunice.Analysis.Files.TypeItem

let rec createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    let itemsOfType =
        let methodsOfType =
            match ``type``.BaseType with
            | null -> seq []
            | baseType when baseType.FullName = "System.MulticastDelegate" -> seq []
            | _ -> createItemsFromMethods ``type``.Methods

        seq [
            yield! ``type``.NestedTypes |> Seq.map createItemFromType 
            yield! methodsOfType
        ]
        |> Seq.toList

    {
        Identifier = ``type``.Name
        Items = itemsOfType
    }

and private createItemsFromMethods =
    Seq.filter (fun method -> method.IsConstructor |> not)
    >> Seq.map (fun method -> { Identifier = method.Name; Items = [] })