set -e

projs=("JF.Identity.API")
for arg in $projs
do
    dotnet build -c Release ./src/$arg
done
