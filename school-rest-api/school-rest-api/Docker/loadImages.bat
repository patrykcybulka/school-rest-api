docker image rm --force school-rest-api

docker build -f "..\Dockerfile" -t school-rest-api --force-rm ..\..

docker image save -o "school-rest-api-image.tar" school-rest-api:latest

docker pull postgres
docker pull redis
