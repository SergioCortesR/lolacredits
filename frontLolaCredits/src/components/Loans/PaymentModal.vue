<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 flex items-center justify-center z-60">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl dark:shadow-gray-900/50 border dark:border-gray-700 w-full max-w-md">
      <!-- Header -->
      <div class="px-6 py-4 border-b border-gray-200 dark:border-gray-700">
        <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-100">Registrar Pago</h2>
        <p v-if="installment" class="text-sm text-gray-600 dark:text-gray-400 mt-1">
          Cuota #{{ installment.periodNumber }} - Saldo: ${{ formatCurrency(getBalance()) }}
        </p>
      </div>

      <!-- Form -->
      <form @submit.prevent="handleSubmit" class="p-6 space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Monto a Pagar <span class="text-red-500">*</span>
          </label>
          <input v-model.number="form.amount" type="number" step="0.01" min="0.01" :max="getBalance()" required
            class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400 transition-colors"
            placeholder="0.00" />
          <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
            Máximo: ${{ formatCurrency(getBalance()) }}
          </p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Notas</label>
          <textarea v-model="form.notes" rows="3"
            class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400 transition-colors"
            placeholder="Observaciones del pago (opcional)"></textarea>
        </div>

        <!-- Info Summary -->
        <div v-if="installment" class="bg-gray-50 dark:bg-gray-700/50 p-4 rounded-md space-y-2 text-sm">
          <div class="flex justify-between">
            <span class="text-gray-600 dark:text-gray-400">Monto esperado:</span>
            <span class="font-semibold text-gray-900 dark:text-gray-100">${{ formatCurrency(installment.expectedAmount) }}</span>
          </div>
          <div class="flex justify-between">
            <span class="text-gray-600 dark:text-gray-400">Ya pagado:</span>
            <span class="font-semibold text-gray-900 dark:text-gray-100">${{ formatCurrency(installment.paidAmount) }}</span>
          </div>
          <div class="flex justify-between border-t border-gray-300 dark:border-gray-600 pt-2">
            <span class="text-gray-600 dark:text-gray-400">Saldo pendiente:</span>
            <span class="font-semibold text-red-600 dark:text-red-400">${{ formatCurrency(getBalance()) }}</span>
          </div>
          <div v-if="form.amount > 0" class="flex justify-between border-t border-gray-300 dark:border-gray-600 pt-2">
            <span class="text-gray-600 dark:text-gray-400">Nuevo saldo:</span>
            <span class="font-semibold" :class="getNewBalanceClass()">
              ${{ formatCurrency(getNewBalance()) }}
            </span>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex justify-end space-x-3 pt-2">
          <button type="button" @click="close"
            class="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md text-sm font-medium text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
            Cancelar
          </button>
          <button type="submit" :disabled="submitting"
            class="px-4 py-2 bg-emerald-600 text-white rounded-md text-sm font-medium hover:bg-emerald-700 dark:bg-emerald-500 dark:hover:bg-emerald-600 disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
            {{ submitting ? 'Guardando...' : 'Guardar Pago' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { createPayment } from '@/services/Payment/payments'
import { formatCurrency } from '@/utils/formatters'

const emit = defineEmits(['done'])

const isOpen = ref(false)
const submitting = ref(false)
const installment = ref(null)
const loan = ref(null)

const form = reactive({
  amount: 0,
  notes: ''
})

const open = (installmentData, loanData) => {
  installment.value = installmentData
  loan.value = loanData
  form.amount = getBalance()
  form.notes = ''
  isOpen.value = true
}

const close = () => {
  isOpen.value = false
  installment.value = null
  loan.value = null
  form.amount = 0
  form.notes = ''
}

const getBalance = () => {
  if (!installment.value) return 0
  return installment.value.expectedAmount - installment.value.paidAmount
}

const getNewBalance = () => {
  return getBalance() - (form.amount || 0)
}

const getNewBalanceClass = () => {
  const newBalance = getNewBalance()
  if (newBalance === 0) return 'text-emerald-600 dark:text-emerald-400'
  if (newBalance > 0) return 'text-orange-600 dark:text-orange-400'
  return 'text-gray-700 dark:text-gray-300'
}

const handleSubmit = async () => {
  if (!installment.value) return

  if (form.amount <= 0) {
    alert('El monto debe ser mayor a cero')
    return
  }

  if (form.amount > getBalance()) {
    alert('El monto no puede ser mayor al saldo pendiente')
    return
  }

  submitting.value = true

  try {
    const paymentData = {
      installmentId: installment.value.id,
      amount: form.amount,
      notes: form.notes || null
    }

    await createPayment(paymentData)
    emit('done')
    close()
  } catch (error) {
    const errorMessage = error.response?.data?.message 
      || error.response?.data 
      || 'Error al registrar el pago. Por favor, intente nuevamente.'
    
    alert(errorMessage)
  } finally {
    submitting.value = false
  }
}

defineExpose({ open, close })
</script>
