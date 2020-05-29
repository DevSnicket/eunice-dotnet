module rec DevSnicket.Eunice.Analysis.Types.GetReferencesWhenYieldMethod

open DevSnicket.Eunice.Analysis
open System

let getReferencesWhenYieldMethod (yieldReferencesByTypeName: Map<String, Reference seq>) =
    match yieldReferencesByTypeName.IsEmpty with
    | true ->
        (fun _ -> None)
    | false ->
        fun (method: Mono.Cecil.MethodDefinition) ->
            let rec getReferencesWhenYieldMethod () =
                match method.Body with
                | null ->
                    None
                | body ->
                    body.Instructions
                    |> Seq.tryPick getReferencesOfYieldWhenInstructionOfNew

            and getReferencesOfYieldWhenInstructionOfNew instruction =
                // cSpell:ignore newobj
                if instruction.OpCode = Mono.Cecil.Cil.OpCodes.Newobj then
                    instruction.Operand
                    |> getInstantiatedType
                    |> getReferencesWhenYieldType
                else
                    None
            
            and getInstantiatedType operand =
                (operand :?> Mono.Cecil.MethodReference).DeclaringType

            and getReferencesWhenYieldType instantiatedType =
                let isNested = instantiatedType.DeclaringType = (method.DeclaringType :> Mono.Cecil.TypeReference)

                if isNested then
                    yieldReferencesByTypeName.TryFind instantiatedType.Name
                else
                    None

            getReferencesWhenYieldMethod ()