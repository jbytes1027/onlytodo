import axios from "axios"
import { BACKEND_URL } from "../config"

const baseUrl = `${BACKEND_URL}/tasks`

async function getAll() {
  const response = await axios.get(baseUrl)
  return response.data
}

async function post(title) {
  const response = await axios.post(baseUrl, { title, completed: false })
  return response.data
}

async function remove(id) {
  await axios.delete(`${baseUrl}/${id}`)
}

const taskService = { post, getAll, remove }
export default taskService
