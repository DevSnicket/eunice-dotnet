module rec DevSnicket.Eunice.Analysis.GetReferencesInMethod

open DevSnicket.Eunice.Analysis.GetReferencesInMethodBody
open DevSnicket.Eunice.Analysis.GetReferencesInMethodDeclaration

let getReferencesInMethod (method: Mono.Cecil.MethodDefinition) =
    seq [
        yield! method |> getReferencesInMethodDeclaration
        yield! method.Body |> getReferencesInMethodBody
    ]