#!/bin/bash

set -e

dotnet test \
Analysis/Tests \
-l "trx;LogFileName=.trx" \
-p:AltCover=true \
-p:AltCoverAssemblyFilter="^(?!DevSnicket.Eunice.Analysis$)" \
-p:AltCoverTypeFilter="Program" \
-p:AltCoverForce=true \
-p:AltCoverXmlReport=TestResults/coverage.xml

# ignore error raised when already installed
dotnet tool install dotnet-reportgenerator-globaltool \
--tool-path . \
--version 4.5.6 \
|| true # ignore error raised when already installed

./reportgenerator \
-reports:Analysis/Tests/TestResults/coverage.xml \
-reporttypes:"Html;JsonSummary" \
-targetdir:Analysis/Tests/TestResults/CoverageReport

function getCoverage {
	value=$(grep -Po "(?<=\"$1coverage\": )[\.0-9]*" Analysis/Tests/TestResults/CoverageReport/Summary.json | head -1)
	echo $value%
}
echo
branchcoverage=$(getCoverage "branch")
echo branch coverage: $branchcoverage
linecoverage=$(getCoverage "line")
echo line coverage: $linecoverage
if [ "$branchcoverage" != "100%" ] || [ "$linecoverage" != "100%" ]; then
    exit 1
fi