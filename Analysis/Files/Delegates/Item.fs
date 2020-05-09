module DevSnicket.Eunice.Analysis.Files.Delegates.Item

open DevSnicket.Eunice.Analysis.Files

let private createDependsUponFromMethods =
    TypesInMethods.getTypesInMethods
    >> DependsUponTypes.createDependsUponFromTypes

let createItemFromDelegate (``delegate``: Mono.Cecil.TypeDefinition) =
    {
        DependsUpon = createDependsUponFromMethods ``delegate``.Methods
        Identifier = ``delegate``.Name
        Items = []
    }
