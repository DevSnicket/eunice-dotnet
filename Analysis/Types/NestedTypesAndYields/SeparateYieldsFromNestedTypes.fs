module rec DevSnicket.Eunice.Analysis.Types.NestedTypesAndYields.SeparateYieldsFromNestedTypes

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.Types.NestedTypesAndYields.GetReferencesWhenYieldGeneratedType
open System

type NestedTypesAndYields =
    {
        NestedTypes: Mono.Cecil.TypeDefinition seq
        YieldReferencesByTypeName: Map<String, Reference seq>
    }

type private NestedTypeOrYield =
    | NestedType of Mono.Cecil.TypeDefinition
    | Yield of String * Reference seq

let separateYieldsFromNestedTypes (nestedTypes: Mono.Cecil.TypeDefinition seq) =
    let nestedItemsAndYields =
        nestedTypes
        |> Seq.map createItemOrYieldFromNestedType
        |> Seq.toList

    {
        NestedTypes =
            nestedItemsAndYields
            |> Seq.choose (
                function
                | NestedType nestedType -> Some nestedType
                | Yield -> None
            )
        YieldReferencesByTypeName =
            nestedItemsAndYields
            |> Seq.choose (
                function
                | NestedType -> None
                | Yield (typeName, references) -> Some (typeName, references)
            )
            |> Map.ofSeq
    }

let private createItemOrYieldFromNestedType ``type`` =
    ``type``
    |> getReferencesWhenYieldGeneratedType
    |> function
        | Some references ->
            Yield (``type``.Name, references)
        | None ->
            NestedType ``type``