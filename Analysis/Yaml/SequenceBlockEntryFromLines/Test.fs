module DevSnicket.Eunice.Analysis.Tests.Yaml.SequenceBlockEntryFromLines.Test

open DevSnicket.Eunice.Analysis.Yaml.SequenceBlockEntryFromLines
open System

// following behaviour is impossible to recreate in file-system/integration test cases

[<Xunit.Fact>]
let BlockSequenceLinesOfEmpty () =
    Xunit.Assert.Equal<String seq>(
        seq [],
        sequenceBlockEntryFromLines []
    )