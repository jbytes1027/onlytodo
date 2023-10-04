import { useEffect, useState } from "react"
import taskService from "../services/TaskService"
import AddTodoItem from "./AddTodoItem"

const TodoList = () => {
  const [list, setList] = useState([])

  function updateList() {
    taskService
      .getAll()
      .then((res) => setList(res))
      .catch((err) => console.log(err))
  }

  async function onDeleteTodo(id) {
    try {
      await taskService.remove(id)
      updateList()
    } catch (err) {
      console.log(err)
    }
  }

  useEffect(() => {
    updateList()
  }, [])

  return (
    <div className="todo-list">
      {list.map((e) => (
        <TodoItem
          title={e.title}
          key={e.id}
          onDelete={() => onDeleteTodo(e.id)}
        />
      ))}
      <AddTodoItem onSubmit={updateList} />
    </div>
  )
}

const TodoItem = (props) => {
  return (
    <div className="todo-list-item">
      <div className="title">{props.title}</div>
      <div className="button-delete" onClick={props.onDelete}>&#x00d7;</div>
    </div>
  )
}

export default TodoList
