module rec DevSnicket.Eunice.Analysis.Files.Methods.TypesReferenced

let getTypesReferencedByMethod (method: Mono.Cecil.MethodDefinition) =
    let isDeclaringType (``type``: Mono.Cecil.TypeReference) =
        ``type``.FullName = method.DeclaringType.FullName

    seq [
        yield! method.Parameters |> Seq.map getTypeFromParameter
        yield! method.ReturnType |> getTypesFromReturnType
        yield! method.Body |> getTypesFromBody
    ]
    |> Seq.filter (isDeclaringType >> not)
    |> distinctTypes

let private getTypeFromParameter parameter =
    parameter.ParameterType
    
let private getTypesFromReturnType returnType =
    match returnType.FullName with
    | "System.Void" -> []
    | _ -> [ returnType ]

let private getTypesFromBody body =
    match body with
    | null ->
        seq []
    | _ ->
        body.Variables
        |> Seq.map (fun variable -> variable.VariableType)

let private distinctTypes =
    Seq.distinctBy (fun ``type`` -> ``type``.FullName)