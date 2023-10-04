import { useState } from "react"
import taskService from "../services/TaskService"

function AddTodoItem(props) {
  const [inputText, setInputText] = useState("")
  const [isLoading, setIsLoading] = useState(false)

  function onKeyDown(event) {
    if (event.key === "Enter") {
      onSubmitAsync()
    }
  }

  async function onSubmitAsync() {
    console.log("Adding new element")

    setIsLoading(true)
    try {
      await taskService.post(inputText)
      setInputText("")
      props.onSubmit()
    } catch (err) {
      console.log(err)
    }
    setIsLoading(false)
  }

  return (
    <div className="add-todo">
      <input
        type="text"
        value={inputText}
        onChange={(e) => setInputText(e.target.value)}
        onKeyDown={onKeyDown}
        disabled={isLoading}
      />
      <input
        type="button"
        value="Add"
        onClick={onSubmitAsync}
        disabled={isLoading}
      />
    </div>
  )
}

export default AddTodoItem
