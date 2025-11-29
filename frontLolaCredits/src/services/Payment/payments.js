import axios from '@/axios'

// Create new payment
export const createPayment = async (paymentData) => {
  const response = await axios.post('/api/payments', paymentData)
  return response.data
}
