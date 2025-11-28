import api from '@/axios'

// Get all payments
export const getPayments = () => {
  return api.get('/payments')
}

// Get payments by loan ID
export const getPaymentsByLoan = (loanId) => {
  return api.get(`/payments/loan/${loanId}`)
}

// Get payment by ID
export const getPayment = (id) => {
  return api.get(`/payments/${id}`)
}

// Create new payment
export const createPayment = (payment) => {
  return api.post('/payments', payment)
}
