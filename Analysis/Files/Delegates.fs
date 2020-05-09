module rec DevSnicket.Eunice.Analysis.Files.Delegates.Item

open DevSnicket.Eunice.Analysis.Files

let createItemFromDelegate (``delegate``: Mono.Cecil.TypeDefinition) =
    {
        DependsUpon = createDependsUponFromMethods ``delegate``.Methods
        Identifier = ``delegate``.Name
        Items = []
    }

let private createDependsUponFromMethods methods =
    methods
    |> Seq.find isInvokeMethod
    |> Methods.DependsUpon.createDependsUponFromMethod

let private isInvokeMethod method =
    method.Name = "Invoke"