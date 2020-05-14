module rec DevSnicket.Eunice.Analysis.Files.Methods.References

open DevSnicket.Eunice.Analysis.Files

let getReferencesOfMethod (method: Mono.Cecil.MethodDefinition) =
    let isDeclaringType reference =
        match reference with
        | TypeReference ``type`` -> ``type``.FullName = method.DeclaringType.FullName
        | MethodReference _ -> false

    seq [
        yield! method.Parameters |> Seq.map getTypeFromParameter
        yield! method.ReturnType |> getTypesFromReturnType
        yield! method.Body |> getReferencesFromBody
    ]
    |> Seq.filter (isDeclaringType >> not)

let private getTypeFromParameter parameter =
    TypeReference parameter.ParameterType

let private getTypesFromReturnType returnType =
    match returnType.FullName with
    | "System.Void" -> seq []
    | _ -> seq [ TypeReference returnType ]

let private getReferencesFromBody body =
    match body with
    | null ->
        seq []
    | _ ->
        seq [
            yield! body.Instructions |> Seq.choose Instructions.getMethodUsedByInstruction
            yield! body.Variables |> Seq.map (fun variable -> TypeReference variable.VariableType)
        ]