module DevSnicket.Eunice.Analysis.Files.BaseNamespace

open System

type TypeWithNamespaceSegments =
 {
  Name: String
  NamespaceSegments: String list
 }

type private BaseNamespaceWithTypes =
 {
  BaseNamespace: String
  Type: TypeWithNamespaceSegments
 }

let private dequeueBaseNamespaceFromType ``type`` =
 match ``type``.NamespaceSegments with
 | head :: tail ->
  {
   BaseNamespace = head
   Type = { ``type`` with NamespaceSegments = tail }
  }
 | [] ->
  {
   BaseNamespace = ""
   Type = ``type``
  }

let groupTypes types =
 types
 |> Seq.map dequeueBaseNamespaceFromType
 |> Seq.groupBy (fun ``type`` -> ``type``.BaseNamespace)
 |> Seq.map (
  fun (``namespace``, typesWithBaseNamespace) -> 
   (
    ``namespace``,
    typesWithBaseNamespace |> Seq.map(fun typeBaseNamespace -> typeBaseNamespace.Type)
   )
  )
