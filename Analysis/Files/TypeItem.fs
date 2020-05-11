module rec DevSnicket.Eunice.Analysis.Files.TypeItem

let createItemFromType (``type``: Mono.Cecil.TypeDefinition) =
    match ``type``.BaseType with
    | null ->
        {
            DependsUpon =
                ``type``.Interfaces
                |> getTypesOfInterfaces
                |> createDependsUponFromReferencedTypes
            Identifier =
                ``type``.Name
            Items =
                ``type`` |> createItemsFromType
        }
    | baseType when baseType.FullName = "System.MulticastDelegate" ->
        Delegates.Item.createItemFromDelegate ``type``
    | baseType ->
        {
            DependsUpon =
                seq [
                    baseType;
                    yield! ``type``.GenericParameters |> getTypesOfGenericParameters
                    yield! ``type``.Interfaces |> getTypesOfInterfaces
                ]
                |> createDependsUponFromReferencedTypes
            Identifier =
                ``type``.Name
            Items =
                ``type`` |> createItemsFromType
        }

let private getTypesOfGenericParameters parameters =
    parameters
    |> Seq.collect (fun parameter -> parameter.Constraints |> getTypesOfGenericConstraints)

let private getTypesOfGenericConstraints =
    Seq.map (fun ``constraint`` -> ``constraint``.ConstraintType)

let private getTypesOfInterfaces =
    Seq.map (fun ``interface`` -> ``interface``.InterfaceType)

let private createDependsUponFromReferencedTypes types =
    types
    |> Seq.filter isDependUponTypeRelevant
    |> DependsUponTypes.createDependsUponFromTypes

let private isDependUponTypeRelevant ``type`` =
    [ "System.Enum"; "System.Object" ]
    |> List.contains ``type``.FullName
    |> not

let private createItemsFromType ``type`` =
    [
        yield! ``type``.NestedTypes |> Seq.map createItemFromType
        yield! ``type``.Methods |> Seq.collect createItemsFromMethod
    ]

let private createItemsFromMethod method =
    let dependsUpon = method |> Methods.DependsUpon.createDependsUponFromMethod

    match (dependsUpon, method.IsConstructor) with
    | ([], true) ->
        seq []
    | _ ->
        seq [
            {
                DependsUpon = dependsUpon
                Identifier = method.Name
                Items = []
            }
        ]