find . -type f \( -name "*.cs" -o -name "*.fs" -o -path ./website/dotnet/git-log.txt \) ! -path "**/obj/**" -exec npx cspell@4.0.30 {} +