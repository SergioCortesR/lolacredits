import axios from '@/axios'

// Get installments by loan ID
export const getInstallmentsByLoan = async (loanId) => {
  const response = await axios.get(`/api/installments/loan/${loanId}`)
  return response.data
}
