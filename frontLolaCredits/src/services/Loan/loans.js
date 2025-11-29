import api from '@/axios'

// Get all loans (with pagination and filters)
export const getLoans = (params) => {
  return api.get('api/loans', { params })
}

// Get all loans without pagination
export const getAllLoans = () => {
  return api.get('api/loans/all')
}

// Get loans by person ID
export const getLoansByPerson = (personId) => {
  return api.get(`api/loans/person/${personId}`)
}

// Get loan by ID
export const getLoan = (id) => {
  return api.get(`api/loans/${id}`)
}

// Create new loan
export const createLoan = (loan) => {
  return api.post('api/loans', loan)
}

// Update loan
export const updateLoan = (id, loan) => {
  return api.put(`api/loans/${id}`, loan)
}

// Delete loan
export const deleteLoan = (id) => {
  return api.delete(`api/loans/${id}`)
}
