import api from '@/axios'

// Get all installments
export const getInstallments = () => {
  return api.get('api/installments')
}

// Get installments by loan ID
export const getInstallmentsByLoan = (loanId) => {
  return api.get(`api/installments/loan/${loanId}`)
}

// Get pending installments by loan ID
export const getPendingInstallmentsByLoan = (loanId) => {
  return api.get(`api/installments/loan/${loanId}/pending`)
}

// Get installment by ID
export const getInstallment = (id) => {
  return api.get(`api/installments/${id}`)
}
