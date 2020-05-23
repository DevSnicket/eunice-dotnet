module rec DevSnicket.Eunice.Analysis.CreateItemFromDelegate

open DevSnicket.Eunice.Analysis.CreateDependsUponFromReferencesAndReferrer
open DevSnicket.Eunice.Analysis.Methods.GetReferencesOfMethod

let createItemFromDelegate (``delegate``: Mono.Cecil.TypeDefinition) =
    let rec createItemFromDelegate () =
        {
            DependsUpon =
                ``delegate``.Methods
                |> getReferencesOfMethods
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

let private getReferencesOfMethods methods =
    methods
    |> Seq.find isInvokeMethod
    |> getReferencesOfMethod

let private isInvokeMethod method =
    method.Name = "Invoke"