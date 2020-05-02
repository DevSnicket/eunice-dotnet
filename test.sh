#!/bin/bash
dotnet test \
Analysis/Files/Tests \
-l trx \
-p:AltCover=true \
-p:AltCoverAssemblyFilter="^(?!DevSnicket.Eunice.Analysis.Files$)" \
-p:AltCoverForce=true \
-p:AltCoverThreshold=100 \
-p:AltCoverXmlReport=TestResults/coverage.xml

dotnet tool install dotnet-reportgenerator-globaltool \
--tool-path . \
--version 4.5.6

./reportgenerator \
-reports:Analysis/Files/Tests/TestResults/coverage.xml \
-targetdir:Analysis/Files/Tests/TestResults/CoverageReport