import api from '@/axios'

// Get all persons
export const getPersons = () => {
  return api.get('/persons')
}

// Get person by ID
export const getPerson = (id) => {
  return api.get(`/persons/${id}`)
}

// Create new person
export const createPerson = (person) => {
  return api.post('/persons', person)
}

// Update person
export const updatePerson = (id, person) => {
  return api.put(`/persons/${id}`, person)
}

// Delete person
export const deletePerson = (id) => {
  return api.delete(`/persons/${id}`)
}
