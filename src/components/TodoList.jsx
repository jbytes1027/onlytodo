const TodoList = () => {
    const list = [
        { id: "1a", title: "task 1", completed: false },
        { id: "2a", title: "task 2", completed: false },
        { id: "3a", title: "task 3", completed: true },
    ];

    return (
        <div className="todo-list">
            {list.map((e) => (
                <TodoItem title={e.title} />
            ))}
        </div>
    );
};

const TodoItem = (props) => {
    return (
        <div className="todo-list-item">
            <div className="checkbox" />
            <div className="title">{props.title}</div>
        </div>
    );
};

export default TodoList;
