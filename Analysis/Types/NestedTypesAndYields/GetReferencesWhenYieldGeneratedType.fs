module rec DevSnicket.Eunice.Analysis.Types.NestedTypesAndYields.GetReferencesWhenYieldGeneratedType

open DevSnicket.Eunice.Analysis
open DevSnicket.Eunice.Analysis.GetReferencesInMethodBody

let getReferencesWhenYieldGeneratedType (``type``: Mono.Cecil.TypeDefinition) =
    let rec createItemWhenYieldType () =
        match isYieldType with
        | true -> Some (getReferencesFromYieldType ())
        | false -> None

    and isYieldType =
        ``type``.CustomAttributes |> Seq.exists isCompilerGeneratedCustomAttribute
        &&
        ``type``.Interfaces |> Seq.exists isInterfaceOfIEnumerable

    and getReferencesFromYieldType () =
        seq [
            yield! getReferencesFromMethods ()
            yield! getReferencesFromFields ()
        ]

    and getReferencesFromMethods () =
        ``type``.Methods
        |> Seq.find isMoveNextMethod
        |> getReferencesFromMoveNextMethod

    and getReferencesFromMoveNextMethod method =
        method.Body
        |> getReferencesInMethodBody
        |> Seq.filter (referenceToImplementation >> not)

    and referenceToImplementation =
        function
        | FieldReference fieldReference -> fieldReference.DeclaringType.FullName = ``type``.FullName
        | _ -> false

    and getReferencesFromFields () =
        ``type``.Fields
        |> Seq.choose getReferenceFromFieldWhenVariable

    createItemWhenYieldType ()

let private isCompilerGeneratedCustomAttribute attribute =
    attribute.AttributeType.FullName = "System.Runtime.CompilerServices.CompilerGeneratedAttribute"

let private isInterfaceOfIEnumerable ``interface`` =
    ``interface``.InterfaceType.FullName = "System.Collections.IEnumerable"

let private isMoveNextMethod method =
    method.Name = "MoveNext"

let private getReferenceFromFieldWhenVariable field =
    match field.Name.StartsWith "<" && field.Name.Contains ">5__" with
    | false -> None
    | true -> Some (TypeReference field.FieldType)