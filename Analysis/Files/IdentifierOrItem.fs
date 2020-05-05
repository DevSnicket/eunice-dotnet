namespace DevSnicket.Eunice.Analysis.Files

open System

type IdentifierOrItem =
 | Identifier of String
 | Item of Item
and Item =
 {
  Identifier: String
  Items: IdentifierOrItem list
 }

type IdentifierOrItemWithNamespaceSegments =
 {
  IdentifierOrItem: IdentifierOrItem
  NamespaceSegments: String list
 }