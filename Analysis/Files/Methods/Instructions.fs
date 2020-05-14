module rec DevSnicket.Eunice.Analysis.Files.Methods.Instructions

open DevSnicket.Eunice.Analysis.Files

let getMethodsUsedByInstruction (instruction: Mono.Cecil.Cil.Instruction) =
    match instruction.Operand with
    | :? Mono.Cecil.MethodReference as methodReference ->
        seq [ MethodReference(methodReference) ]
    | _ ->
        seq [ ]