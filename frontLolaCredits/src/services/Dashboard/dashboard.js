import axios from '@/axios'

// Get dashboard statistics
export const getDashboardStats = async () => {
  const response = await axios.get('/api/dashboard/stats')
  return response.data
}
