import api from '@/axios'

// Get all loans
export const getLoans = () => {
  return api.get('/loans')
}

// Get loans by person ID
export const getLoansByPerson = (personId) => {
  return api.get(`/loans/person/${personId}`)
}

// Get loan by ID
export const getLoan = (id) => {
  return api.get(`/loans/${id}`)
}

// Create new loan
export const createLoan = (loan) => {
  return api.post('/loans', loan)
}

// Update loan
export const updateLoan = (id, loan) => {
  return api.put(`/loans/${id}`, loan)
}

// Delete loan
export const deleteLoan = (id) => {
  return api.delete(`/loans/${id}`)
}
