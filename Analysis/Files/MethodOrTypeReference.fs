namespace DevSnicket.Eunice.Analysis.Files

type MethodOrTypeReference =
    | TypeReference of Mono.Cecil.TypeReference
    | MethodReference of Mono.Cecil.MethodReference