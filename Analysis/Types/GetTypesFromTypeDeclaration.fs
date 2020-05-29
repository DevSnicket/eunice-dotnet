module rec DevSnicket.Eunice.Analysis.Types.GetTypesFromTypeDeclaration

let getTypesFromTypeDeclaration (``type``: Mono.Cecil.TypeDefinition) =
    seq [
        if isNull ``type``.BaseType |> not then
            ``type``.BaseType
        yield! ``type``.GenericParameters |> getTypesOfGenericParameters
        yield! ``type``.Interfaces |> getTypesOfInterfaces
    ]

let private getTypesOfGenericParameters parameters =
    parameters
    |> Seq.collect (fun parameter -> parameter.Constraints |> getTypesOfGenericConstraints)

let private getTypesOfGenericConstraints =
    Seq.map (fun ``constraint`` -> ``constraint``.ConstraintType)

let private getTypesOfInterfaces =
    Seq.map (fun ``interface`` -> ``interface``.InterfaceType)