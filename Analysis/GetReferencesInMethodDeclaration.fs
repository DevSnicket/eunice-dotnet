module rec DevSnicket.Eunice.Analysis.GetReferencesInMethodDeclaration

open DevSnicket.Eunice.Analysis

let getReferencesInMethodDeclaration (method: Mono.Cecil.MethodDefinition) =
    seq [
        yield! method.Parameters |> Seq.map getTypeFromParameter
        yield! method.ReturnType |> getTypesFromReturnType
    ]

let private getTypeFromParameter parameter =
    TypeReference parameter.ParameterType

let private getTypesFromReturnType returnType =
    match returnType.FullName with
    | "System.Void" -> seq []
    | _ -> seq [ TypeReference returnType ]