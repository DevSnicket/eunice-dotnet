module DevSnicket.Eunice.Analysis.File

open Mono.Cecil
open System

let GetTypeNamesFromAssembly (assembly: AssemblyDefinition) =
 assembly.Modules
 |> Seq.collect(fun ``module`` -> ``module``.Types)
 |> Seq.map(fun ``type`` -> ``type``.FullName)
 |> Seq.except([ "<Module>" ])

let Analyze (assemblyFilePath: String) =
 assemblyFilePath
 |> AssemblyDefinition.ReadAssembly
 |> GetTypeNamesFromAssembly
 |> Seq.toArray
 |> SharpYaml.Serialization.Serializer(SharpYaml.Serialization.SerializerSettings(EmitTags = false)).Serialize
 |> fun yaml -> yaml.Replace("\r", "")