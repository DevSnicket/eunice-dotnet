module DevSnicket.Eunice.Analysis.Files.Tests.YamlTest

open System

// following behaviour is impossible to recreate in file-system/integration test cases

[<Xunit.Fact>]
let BlockSequenceLinesOfEmpty() =
    Xunit.Assert.Equal<String seq>(
        seq [],
        DevSnicket.Eunice.Analysis.Files.Yaml.Items.blockSequenceLines []
    )