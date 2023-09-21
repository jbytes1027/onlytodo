docker compose -f 'docker-compose.db.yml' up
docker exec -it dotnet-todo-backend-db-1 /bin/bashdocker-compose.db.yml
psql -d "postgresql://dev@localhost:5432/only_todo_dev?password=dev"