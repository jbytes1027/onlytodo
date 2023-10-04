import TodoList from "./components/TodoList";
import AddTodoItem from "./components/AddTodoItem";
import "./App.css";

function App() {
    return (
        <div className="App">
            <TodoList />
            <AddTodoItem />
        </div>
    );
}

export default App;
