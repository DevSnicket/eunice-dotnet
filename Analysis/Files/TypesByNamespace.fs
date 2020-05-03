module DevSnicket.Eunice.Analysis.Files.TypesByNamespace

let private getTypeName (``type``: Mono.Cecil.TypeDefinition) =
  ``type``.Name

let private getWithIndentForTypes indent =
 let getYamlLinesForType ``type`` =
  match ``type`` with
  | "<Module>" -> seq []
  | _ -> seq [ indent + "- " + ``type`` ]

 Seq.collect(getTypeName >> getYamlLinesForType)

let private getItemForNamespaceAndTypes ``namespace`` types =
 let getWithInlineItems inlineItem =
  seq [
   "- id: " + ``namespace``
   "  items:" + inlineItem
  ];

 let getWithItemsCollection =
  Seq.concat(
   seq [
    getWithInlineItems ""
    getWithIndentForTypes "    " types
   ]
  )

 match types with
 | [ single ] -> getWithInlineItems(" " + getTypeName(single))
 | _ -> getWithItemsCollection

let GetYamlLines (``namespace``, types) =
 match ``namespace`` with
 | "" -> getWithIndentForTypes "" types
 | _ -> getItemForNamespaceAndTypes ``namespace`` (types |> Seq.toList)