
set -e

cd ./test
rootDir=`pwd`
testDirs=`find . -name '*Tests' -type d -maxdepth 1`

for dir in $testDirs
do
    cd $rootDir/$dir
    dotnet restore
    dotnet xunit
done
