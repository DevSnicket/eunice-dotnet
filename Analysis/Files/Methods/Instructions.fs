module rec DevSnicket.Eunice.Analysis.Files.Methods.Instructions

open DevSnicket.Eunice.Analysis.Files

let getMethodUsedByInstruction (instruction: Mono.Cecil.Cil.Instruction) =
    match instruction.Operand with
    | :? Mono.Cecil.MethodReference as methodReference ->
        Some (MethodReference methodReference)
    | _ ->
        None