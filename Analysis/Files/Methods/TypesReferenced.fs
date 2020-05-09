module rec DevSnicket.Eunice.Analysis.Files.Methods.TypesReferenced

let getTypesReferencedByMethod (method: Mono.Cecil.MethodDefinition) =
    seq [
        yield! method.Parameters |> Seq.map getTypeFromParameter
        yield! getTypesFromReturnType method.ReturnType
    ]

let private getTypeFromParameter parameter =
    parameter.ParameterType
    
let private getTypesFromReturnType returnType =
    match returnType.FullName with
    | "System.Void" -> []
    | _ -> [ returnType ]
