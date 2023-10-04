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

  useEffect(() => {
    updateList()
  }, [])

  return (
    <div className="todo-list">
      {list.map((e) => (
        <TodoItem key={e.id} title={e.title} id={e.id} onChange={updateList} />
      ))}
      <AddTodoItem onSubmit={updateList} />
    </div>
  )
}

const TodoItem = (props) => {
  const [isEditMode, setIsEditMode] = useState(false)
  const [newTitle, setNewTitle] = useState("")

  function onKeyDown(event) {
    if (event.key === "Enter") {
      tryUpdateTodo()
    }
  }

  async function onDeleteTodo() {
    try {
      await taskService.remove(props.id)
      props.onChange()
    } catch (err) {
      console.log(err)
    }
  }

  function useEditMode() {
    setNewTitle(props.title)
    setIsEditMode(true)
  }

  async function tryUpdateTodo() {
    try {
      await taskService.update(props.id, newTitle)
      props.onChange()
    } catch (err) {
      console.log(err)
    }

    setIsEditMode(false)
  }
  if (isEditMode)
    return (
      <div className="todo-list-item">
        <input
          type="text"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
          onKeyDown={onKeyDown}
          onBlur={tryUpdateTodo}
        />
      </div>
    )
  else
    return (
      <div className="todo-list-item">
        <div className="title">{props.title}</div>
        <div className="button-edit" onClick={useEditMode}>
          &#x270e;
        </div>
        <div className="button-delete" onClick={onDeleteTodo}>
          &#x00d7;
        </div>
      </div>
    )
}

export default TodoList
