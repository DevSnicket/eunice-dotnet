mkdir website
mkdir website/dotnet

git log \
	--date=short \
	--pretty='format:---%n%ad%n%B' \
	> website/dotnet/git-log.txt