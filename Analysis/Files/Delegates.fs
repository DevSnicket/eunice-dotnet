module rec DevSnicket.Eunice.Analysis.Files.Delegates.Item

open DevSnicket.Eunice.Analysis.Files

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
        DependsUponReferences.createDependsUponFromReferences
            {
                References = references
                ReferrerType = ``delegate``
            }

    createItemFromDelegate ()

let private getReferencesOfMethods methods =
    methods
    |> Seq.find isInvokeMethod
    |> Methods.References.getReferencesOfMethod

let private isInvokeMethod method =
    method.Name = "Invoke"