import { useEffect, useState } from "react"
import TaskService from "../services/TaskService"
import AddTodoItem from "./AddTodoItem"

const TodoList = () => {
  const [list, setList] = useState([])

  function updateList() {
    TaskService.getAll()
      .then((res) => setList(res))
      .catch((err) => console.error(err))
  }

  useEffect(() => {
    updateList()

    // auto update every 3 seconds
    const interval = setInterval(updateList, 3000)
    return () => clearInterval(interval)
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
      await TaskService.remove(props.id)
      props.onChange()
    } catch (err) {
      console.error(err)
    }
  }

  function useEditMode() {
    setNewTitle(props.title)
    setIsEditMode(true)
  }

  async function tryUpdateTodo() {
    try {
      if (newTitle !== props.title) {
        await TaskService.update(props.id, newTitle)
        props.onChange()
      }
    } catch (err) {
      console.error(err)
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
          autoFocus
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
