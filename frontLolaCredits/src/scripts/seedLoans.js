import axios from 'axios'

const API_BASE_URL = 'http://localhost:5228/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

async function seedLoans() {
  console.log('🌱 Starting to seed loans...')

  try {
    // First, get all persons
    const personsResponse = await api.get('/persons')
    const persons = personsResponse.data.items || personsResponse.data

    if (!persons || persons.length === 0) {
      console.log('❌ No persons found. Please create persons first.')
      return
    }

    console.log(`✅ Found ${persons.length} persons`)

    // Create 15 random loans
    const loansToCreate = 15
    let successCount = 0
    let errorCount = 0

    for (let i = 0; i < Math.min(loansToCreate, persons.length); i++) {
      const person = persons[i]
      
      // Generate random loan data
      const amounts = [1000, 2000, 3000, 5000, 7500, 10000, 15000, 20000]
      const months = [3, 6, 12, 18, 24, 36]
      const interestRates = [5, 10, 12, 15, 18, 20]
      const paymentDays = [5, 10, 15, 20, 25]

      const loan = {
        personId: person.id,
        amount: amounts[Math.floor(Math.random() * amounts.length)],
        months: months[Math.floor(Math.random() * months.length)],
        interestRate: interestRates[Math.floor(Math.random() * interestRates.length)],
        paymentDay: paymentDays[Math.floor(Math.random() * paymentDays.length)],
        loanDate: null // Will use current date
      }

      try {
        await api.post('/loans', loan)
        successCount++
        console.log(`✅ Created loan ${successCount}/${loansToCreate}: $${loan.amount} for ${person.fullName}`)
      } catch (error) {
        errorCount++
        console.error(`❌ Error creating loan for ${person.fullName}:`, error.response?.data?.message || error.message)
      }
    }

    console.log('\n📊 Summary:')
    console.log(`✅ Successfully created: ${successCount}`)
    console.log(`❌ Errors: ${errorCount}`)
    console.log('🎉 Seeding completed!')
  } catch (error) {
    console.error('❌ Error:', error.message)
  }
}

seedLoans()
