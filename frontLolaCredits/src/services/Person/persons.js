import api from '@/axios'

// Get all persons
export const getPersons = (params = {}) => {
  return api.get('api/persons', { params });
};

// Get person by ID
export const getPerson = (id) => {
  return api.get(`api/persons/${id}`)
}

// Create new person
export const createPerson = (person) => {
  return api.post('api/persons', person)
}

// Update person
export const updatePerson = (id, person) => {
  return api.put(`api/persons/${id}`, person)
}

// Delete person
export const deletePerson = (id) => {
  return api.delete(`api/persons/${id}`)
}
