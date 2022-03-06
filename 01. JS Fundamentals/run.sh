docker build --tag indexfunc:1.0 .

docker run indexfunc:1.0

docker run --publish 8000:8080 --detach --name indx indexfunc:1.0