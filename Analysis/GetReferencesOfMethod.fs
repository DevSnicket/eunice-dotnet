module rec DevSnicket.Eunice.Analysis.Methods.GetReferencesOfMethod

open DevSnicket.Eunice.Analysis

let getReferencesOfMethod (method: Mono.Cecil.MethodDefinition) =
    seq [
        yield! method.Parameters |> Seq.map getTypeFromParameter
        yield! method.ReturnType |> getTypesFromReturnType
        yield! method.Body |> getReferencesFromBody
    ]

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
            yield! body.Instructions |> Seq.choose getReferenceUsedByInstruction
            yield! body.Variables |> Seq.map (fun variable -> TypeReference variable.VariableType)
        ]

let private getReferenceUsedByInstruction (instruction: Mono.Cecil.Cil.Instruction) =
    match instruction.Operand with
    | :? Mono.Cecil.MethodReference as methodReference ->
        Some (MethodReference methodReference)
    | :? Mono.Cecil.FieldReference as fieldReference ->
        Some (FieldReference fieldReference)
    | :? Mono.Cecil.TypeReference as typeReference ->
        Some (TypeReference typeReference)
    | _ ->
        None