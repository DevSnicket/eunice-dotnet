rm -rf website

git submodule add --force \
	https://$1@github.com/DevSnicket/eunice.git \
	website

./git-log-write-file.sh

cd website

git add dotnet/git-log.txt

git config --add user.email  grahamdyson@hotmail.com
git config --add user.name "GitHub pages publish"

if [[ `git status --porcelain` ]]; then
	git commit -m ".NET Git log";
	git push;
fi