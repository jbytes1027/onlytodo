CREATE DATABASE only_todo_dev;

CREATE TABLE tasks (
    id UUID PRIMARY KEY,
    title VARCHAR NOT NULL,
    completed BOOL NOT NULL
);
