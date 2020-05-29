module rec DevSnicket.Eunice.Analysis.Types.GetReferencesFromConstructor

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.GetReferencesInMethod

let getReferencesFromConstructor (constructor: Mono.Cecil.MethodDefinition) =
    constructor
    |> getReferencesInMethod
    |> Seq.filter (isReferenceToParameterlessConstructor >> not)

let isReferenceToParameterlessConstructor =
    function
    | FieldReference _
    | TypeReference _ ->
        false
    | MethodReference method ->
        method.Name = ".ctor" && method.Parameters.Count = 0