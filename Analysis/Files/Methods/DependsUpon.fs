module DevSnicket.Eunice.Analysis.Files.Methods.DependsUpon

open DevSnicket.Eunice.Analysis.Files

let createDependsUponFromMethod =
    TypesReferenced.getTypesReferencedByMethod
    >> DependsUponTypes.createDependsUponFromTypes