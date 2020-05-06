module DevSnicket.Eunice.Analysis.Files.Tests.YamlTest

open System

// Following behaviour cannot be covered in test cases from file-system tests
[<Xunit.Fact>]
let BlockSequenceLinesAndEmpty() =
    Xunit.Assert.Equal<String seq>(
        seq [],
        DevSnicket.Eunice.Analysis.Files.Yaml.blockSequenceLines []
    )