#!/bin/bash

set -e

dotnet test \
Analysis/Files/Tests \
-l "trx;LogFileName=.trx" \
-p:AltCover=true \
-p:AltCoverAssemblyFilter="^(?!DevSnicket.Eunice.Analysis.Files$)" \
-p:AltCoverForce=true \
-p:AltCoverXmlReport=TestResults/coverage.xml

# ignore error raised when already installed
dotnet tool install dotnet-reportgenerator-globaltool \
--tool-path . \
--version 4.5.6 \
|| true # ignore error raised when already installed

./reportgenerator \
-reports:Analysis/Files/Tests/TestResults/coverage.xml \
-reporttypes:"Html;JsonSummary" \
-targetdir:Analysis/Files/Tests/TestResults/CoverageReport

grep -Pzo '"linecoverage": 100[^}]*"branchcoverage": 100' Analysis/Files/Tests/TestResults/CoverageReport/Summary.json