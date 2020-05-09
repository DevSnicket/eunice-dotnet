module rec DevSnicket.Eunice.Analysis.Files.Delegates.TypesInMethods

let getTypesInMethods (methods: Mono.Cecil.MethodDefinition seq) =
    methods
    |> Seq.find isInvokeMethod
    |> getTypesFromInvoke

let private isInvokeMethod method =
    method.Name = "Invoke"

let private getTypesFromInvoke invoke =
    seq [
        yield! invoke.Parameters |> Seq.map getTypeFromParameter
        yield! getTypesFromReturnType invoke.ReturnType
    ]

let private getTypeFromParameter parameter =
    parameter.ParameterType
    
let private getTypesFromReturnType returnType =
    match returnType.FullName with
    | "System.Void" -> []
    | _ -> [ returnType ]
