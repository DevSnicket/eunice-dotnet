module rec DevSnicket.Eunice.Analysis.CreateItemFromDelegate

open DevSnicket.Eunice.Analysis.CreateDependsUponFromReferencesAndReferrer
open DevSnicket.Eunice.Analysis.GetReferencesInMethod

let createItemFromDelegate (``delegate``: Mono.Cecil.TypeDefinition) =
    let rec createItemFromDelegate () =
        {
            DependsUpon =
                ``delegate``.Methods
                |> getReferencesInMethods
                |> createDependsUponFromReferences
            Identifier =
                ``delegate``.Name
            Items =
                []
        }

    and createDependsUponFromReferences references =
        createDependsUponFromReferencesAndReferrer
            {
                References = references
                ReferrerType = ``delegate``
            }

    createItemFromDelegate ()

let private getReferencesInMethods methods =
    methods
    |> Seq.find isInvokeMethod
    |> getReferencesInMethod

let private isInvokeMethod method =
    method.Name = "Invoke"