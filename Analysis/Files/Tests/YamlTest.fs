module DevSnicket.Eunice.Analysis.Files.Tests.YamlTest

open DevSnicket.Eunice.Analysis.Files
open System

// following behaviour is impossible to recreate in file-system/integration test cases

[<Xunit.Fact>]
let BlockSequenceLinesOfEmpty() =
    Xunit.Assert.Equal<String seq>(
        seq [],
        Yaml.blockSequenceLines []
    )

[<Xunit.Fact>]
let LinesForChildItemsOfEmpty() =
    Xunit.Assert.Equal<String seq>(
        seq [],
        Yaml.linesForChildItems []
    )

[<Xunit.Fact>]
let LinesForChildItemsMappingOfEmpty() =
    Xunit.Assert.Equal<String seq>(
        seq [],
        Yaml.linesForChildItemsMapping []
    )