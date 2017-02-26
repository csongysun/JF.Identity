
$name="identity-api"

$reg="192.168.31.10:5000";

docker-machine.exe env -u | iex;

docker-compose.exe -f .\docker-compose.ci.build.yml up;

docker-compose.exe -f .\docker-compose.swarm.yml build;

$tag="$reg/$name";

docker tag $name $tag;

docker image push $tag;

docker-machine.exe env vm0 | iex;

docker stack deploy -c .\docker-compose.swarm.yml identity

Read-Host -Prompt "Press Enter to exit";
