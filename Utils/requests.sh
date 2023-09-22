exit 0

http GET ":5057/tasks"
http -v POST ":5057/tasks" \
    title="Do the Laundry" \
    completed:=false \
    id=a4d7a200-83b2-4a30-aa15-3d3d9b1cb46f