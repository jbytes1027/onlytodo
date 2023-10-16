docker compose -f 'docker-compose.db.yml' up
docker exec -it onlytodo-server-db-1 /bin/bashdocker-compose.db.yml
psql -d "postgresql://dev@localhost:5432/only_todo_dev?password=dev"
dotnet ef dbcontext scaffold "Host=localhost; Database=only_todo_dev; Username=dev; Password=dev" Npgsql.EntityFrameworkCore.PostgreSQL