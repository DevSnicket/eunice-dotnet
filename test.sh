dotnet test \
Analysis/File/Tests \
-l trx \
-p:AltCover=true \
-p:AltCoverAssemblyFilter="^(?!DevSnicket.Eunice.Analysis.File$)" \
-p:AltCoverForce=true \
-p:AltCoverThreshold=100 \
-p:AltCoverXmlReport=TestResults/coverage.xml

dotnet tool install dotnet-reportgenerator-globaltool --tool-path . --version 4.5.6
./reportgenerator -reports:Analysis/File/Tests/TestResults/coverage.xml -targetdir:Analysis/File/Tests/TestResults/CoverageReport