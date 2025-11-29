<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 rounded-xl flex items-center justify-center z-10">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-2xl">
      <h2 class="text-lg font-semibold text-gray-900 mb-4">
        {{ editingLoan ? 'Editar Préstamo' : 'Nuevo Préstamo' }}
      </h2>

      <div v-if="error" class="mb-4 bg-red-50 border border-red-200 text-red-700 px-3 py-2 rounded text-sm">
        {{ error }}
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <!-- Person Selection -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            Cliente <span class="text-red-500">*</span>
          </label>
          <select v-model="form.personId" required
            :class="[
              'w-full px-4 py-2 border rounded-lg focus:outline-none',
              editingLoan 
                ? 'bg-gray-100 border-gray-200 cursor-not-allowed text-gray-500' 
                : 'border-gray-300 focus:ring-2 focus:ring-blue-500'
            ]"
            :disabled="editingLoan">
            <option value="">Seleccionar cliente...</option>
            <option v-for="person in persons" :key="person.id" :value="person.id">
              {{ person.fullName }} - CI: {{ person.ci }}
            </option>
          </select>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <Input v-model="form.amount" label="Monto Préstamo" type="number" step="0.01" placeholder="1000.00" required
            :disabled="editingLoan" />
          <Input v-model="form.months" label="Meses" type="number" placeholder="12" required :disabled="editingLoan" />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <Input v-model="form.interestRate" label="Interés (%)" type="number" placeholder="10" required />
          <Input v-model="form.paymentDay" label="Día de Cobro" type="number" min="1" max="28" placeholder="15"
            required />
        </div>

        <Input v-model="form.loanDate" label="Fecha de Préstamo" type="date" :disabled="editingLoan" />

        <div v-if="editingLoan" class="bg-blue-50 border border-blue-200 px-3 py-2 rounded text-sm text-blue-700">
          ℹ️ Solo puedes editar el interés y el día de cobro. El monto y meses no pueden modificarse.
        </div>

        <div class="flex gap-3 justify-end pt-4">
          <button type="button" @click="close"
            class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">
            Cancelar
          </button>
          <button type="submit" :disabled="isLoading"
            class="px-4 py-2 bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white rounded-lg transition-colors">
            {{ isLoading ? 'Guardando...' : 'Guardar' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import Input from '@/components/Form/Input.vue'
import { getLoan, createLoan, updateLoan } from '@/services/Loan/loans'
import { getPersons } from '@/services/Person/persons'

const emit = defineEmits(['done'])

const isOpen = ref(false)
const isLoading = ref(false)
const error = ref('')
const editingLoan = ref(null)
const persons = ref([])

const form = ref({
  personId: '',
  amount: '',
  months: '',
  interestRate: '',
  paymentDay: '',
  loanDate: ''
})

onMounted(async () => {
  await loadPersons()
})

const loadPersons = async () => {
  try {
    const response = await getPersons({ pageSize: 1000 })
    persons.value = response.data.items || response.data
  } catch (err) {
    console.error('Error loading persons:', err)
  }
}

const open = async (loan = null) => {
  editingLoan.value = loan
  if (loan) {
    try {
      const response = await getLoan(loan.id)
      const loanData = response.data
      form.value = {
        personId: loanData.personId,
        amount: loanData.amount,
        months: loanData.months,
        interestRate: loanData.interestRate,
        paymentDay: loanData.paymentDay,
        loanDate: loanData.loanDate
      }
    } catch (err) {
      console.error(err)
      error.value = "Error loading loan"
    }
  } else {
    const today = new Date().toISOString().split('T')[0]
    form.value = {
      personId: '',
      amount: '',
      months: '',
      interestRate: '',
      paymentDay: '15',
      loanDate: today
    }
  }
  error.value = ""
  isOpen.value = true
}

const close = () => {
  isOpen.value = false
  form.value = {
    personId: '',
    amount: '',
    months: '',
    interestRate: '',
    paymentDay: '',
    loanDate: ''
  }
}

const submit = async () => {
  isLoading.value = true
  error.value = ''

  try {
    const payload = {
      personId: parseInt(form.value.personId),
      amount: parseFloat(form.value.amount),
      months: parseInt(form.value.months),
      interestRate: parseInt(form.value.interestRate),
      paymentDay: parseInt(form.value.paymentDay),
      loanDate: form.value.loanDate || null
    }

    if (editingLoan.value) {
      await updateLoan(editingLoan.value.id, payload)
    } else {
      await createLoan(payload)
    }
    emit('done')
    close()
  } catch (err) {
    error.value = err.response?.data?.message || 'Error saving loan'
    console.error(err)
  } finally {
    isLoading.value = false
  }
}

defineExpose({ open })
</script>
