module DevSnicket.Eunice.Analysis.Files.NamespaceAndTypeHierarchy

let private identifiersFromTypes =
 Seq.map(fun ``type`` -> Identifier(``type``.Name))

let rec groupTypesAndNamespaceSegments types =
 types
 |> BaseNamespace.groupTypes
 |> Seq.collect groupTypesInNamespace
 |> Seq.toList

and private groupTypesInNamespace (``namespace``, types) =
 let itemsFromNamespace() =
  seq [ Item({
   Identifier = ``namespace``
   Items = groupTypesAndNamespaceSegments(types)
  }) ]

 match ``namespace`` with
 | "" -> types |> identifiersFromTypes
 | _ -> itemsFromNamespace()