import api from '@/axios'

// Get all loans
export const getLoans = () => {
  return api.get('api/loans')
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
