namespace DevSnicket.Eunice.Analysis.Files

type Reference =
    | FieldReference of Mono.Cecil.FieldReference
    | MethodReference of Mono.Cecil.MethodReference
    | TypeReference of Mono.Cecil.TypeReference