<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 rounded-xl flex items-center justify-center z-10">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 w-full max-w-2xl">
      <h2 class="text-lg font-semibold text-gray-900 dark:text-gray-100 mb-4">
        {{ editingLoan ? 'Editar Préstamo' : 'Nuevo Préstamo' }}
      </h2>

      <div v-if="error" class="mb-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-700 dark:text-red-400 px-3 py-2 rounded text-sm">
        {{ error }}
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <!-- Person Selection -->
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Cliente <span class="text-red-500 dark:text-red-400">*</span>
          </label>
          <select v-model="form.personId" required
            :class="[
              'w-full px-4 py-2 border rounded-lg focus:outline-none',
              editingLoan 
                ? 'bg-gray-100 dark:bg-gray-700 border-gray-200 dark:border-gray-600 cursor-not-allowed text-gray-500 dark:text-gray-400' 
                : 'bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 border-gray-300 dark:border-gray-600 focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400'
            ]"
            :disabled="editingLoan">
            <option value="">Seleccionar cliente...</option>
            <option v-for="person in persons" :key="person.id" :value="person.id">
              {{ person.fullName }} - CI: {{ person.ci }}
            </option>
          </select>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <Input v-model="form.amount" label="Monto Préstamo" type="number" step="0.01" placeholder="1000.00" 
            @keypress="onlyNumbers" required :disabled="!!editingLoan" />
          <Input v-model="form.months" label="Meses" type="number" placeholder="12" 
            @keypress="onlyPositiveIntegers" required :disabled="!!editingLoan" />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <Input v-model="form.interestRate" label="Interés (%)" type="number" min="0" step="1" placeholder="10" 
            @keypress="onlyPositiveIntegers" required />
          <Input v-model="form.paymentDay" label="Día de Cobro" type="number" min="1" max="28" placeholder="15"
            @keypress="onlyPositiveIntegers" required />
        </div>

        <Input v-model="form.loanDate" label="Fecha de Préstamo" type="date" :disabled="!!editingLoan" />

        <!-- Calculation Preview (only for create) -->
        <div v-if="!editingLoan && form.amount && form.months && form.interestRate" 
          class="bg-sky-50 dark:bg-sky-900/20 border border-sky-200 dark:border-sky-800 px-4 py-3 rounded-lg text-sm">
          <h3 class="font-semibold text-sky-900 dark:text-sky-300 mb-2">💡 Vista Previa del Cálculo</h3>
          <div class="space-y-1 text-sky-700 dark:text-sky-400">
            <div class="flex justify-between">
              <span>Capital:</span>
              <span class="font-medium">${{ formatCurrency(parseFloat(form.amount)) }}</span>
            </div>
            <div class="flex justify-between">
              <span>Interés mensual ({{ form.interestRate }}%):</span>
              <span class="font-medium">${{ formatCurrency(calculateMonthlyInterest()) }}</span>
            </div>
            <div class="flex justify-between">
              <span>Interés total ({{ form.months }} meses):</span>
              <span class="font-medium">${{ formatCurrency(calculateTotalInterest()) }}</span>
            </div>
            <div class="flex justify-between border-t border-sky-300 dark:border-sky-700 pt-1 mt-1">
              <span class="font-semibold">Total a pagar:</span>
              <span class="font-bold">${{ formatCurrency(calculateTotalAmount()) }}</span>
            </div>
            <div class="flex justify-between border-t border-sky-300 dark:border-sky-700 pt-1 mt-1">
              <span class="font-semibold">Pago mensual:</span>
              <span class="font-bold text-emerald-600 dark:text-emerald-400">${{ formatCurrency(calculateMonthlyPayment()) }}</span>
            </div>
          </div>
        </div>

        <div v-if="editingLoan" class="bg-sky-50 dark:bg-sky-900/20 border border-sky-200 dark:border-sky-800 px-3 py-2 rounded text-sm text-sky-700 dark:text-sky-400">
          ℹ️ Solo puedes editar el interés y el día de cobro. El monto y meses no pueden modificarse.
        </div>

        <div class="flex gap-3 justify-end pt-4">
          <button type="button" @click="close"
            class="px-4 py-2 text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 rounded-lg transition-colors">
            Cancelar
          </button>
          <button type="submit" :disabled="isLoading"
            class="px-4 py-2 bg-sky-600 dark:bg-sky-500 hover:bg-sky-700 dark:hover:bg-sky-600 disabled:bg-gray-400 dark:disabled:bg-gray-600 text-white rounded-lg transition-colors">
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
import { formatCurrency } from '@/utils/formatters'

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
    // Error handled silently
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
      error.value = "Error al cargar préstamo"
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
      interestRate: parseFloat(form.value.interestRate),
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
  } finally {
    isLoading.value = false
  }
}

// Calculation helpers (Simple Monthly Interest)
const calculateMonthlyInterest = () => {
  const amount = parseFloat(form.value.amount) || 0
  const rate = parseFloat(form.value.interestRate) || 0
  return amount * (rate / 100)
}

const calculateTotalInterest = () => {
  const months = parseInt(form.value.months) || 0
  return calculateMonthlyInterest() * months
}

const calculateTotalAmount = () => {
  const amount = parseFloat(form.value.amount) || 0
  return amount + calculateTotalInterest()
}

const calculateMonthlyPayment = () => {
  const amount = parseFloat(form.value.amount) || 0
  const months = parseInt(form.value.months) || 1
  const capitalPerMonth = amount / months
  return capitalPerMonth + calculateMonthlyInterest()
}

const onlyPositiveIntegers = (event) => {
  const char = String.fromCharCode(event.keyCode || event.which)
  // Solo permite números (0-9)
  if (!/^[0-9]$/.test(char)) {
    event.preventDefault()
  }
}

const onlyNumbers = (event) => {
  const char = String.fromCharCode(event.keyCode || event.which)
  // Permite números (0-9) y punto decimal (.)
  if (!/^[0-9.]$/.test(char)) {
    event.preventDefault()
  }
}

defineExpose({ open })
</script>
