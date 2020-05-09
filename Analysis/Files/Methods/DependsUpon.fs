module DevSnicket.Eunice.Analysis.Files.Methods.DependsUpon

let createDependsUponFromMethod =
    TypesReferenced.getTypesReferencedByMethod
    >> DependsUponTypes.createDependsUponFromTypes