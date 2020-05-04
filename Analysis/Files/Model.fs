module DevSnicket.Eunice.Analysis.Files.Model

open System

type IdentifierOrItem =
 | Identifier of String
 | Item of Item
and Item =
 {
  Identifier: String
  Items: IdentifierOrItem list
 }