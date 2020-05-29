module rec DevSnicket.Eunice.Analysis.GetReferencesInMethodBody

let getReferencesInMethodBody (body: Mono.Cecil.Cil.MethodBody) =
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
    | :? Mono.Cecil.FieldReference as fieldReference ->
        Some (FieldReference fieldReference)
    | :? Mono.Cecil.MethodReference as methodReference ->
        Some (MethodReference methodReference)
    | :? Mono.Cecil.TypeReference as typeReference ->
        Some (TypeReference typeReference)
    | _ ->
        None