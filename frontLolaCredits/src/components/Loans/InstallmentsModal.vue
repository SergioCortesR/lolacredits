<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 flex items-center justify-center z-50">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 w-full max-w-5xl max-h-[90vh] overflow-hidden">
      <!-- Header -->
      <div class="px-6 py-4 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center">
        <div>
          <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-100">Cuotas del Préstamo</h2>
          <p v-if="loan" class="text-sm text-gray-600 dark:text-gray-400 mt-1">
            Cliente: {{ loan.personName }} | Monto: ${{ formatCurrency(loan.amount) }} | {{ loan.months }} meses
          </p>
        </div>
        <button @click="close" class="text-gray-400 dark:text-gray-500 hover:text-gray-600 dark:hover:text-gray-300 transition-colors">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Content -->
      <div class="p-6 overflow-y-auto max-h-[calc(90vh-140px)]">
        <div v-if="loading" class="text-center py-8">
          <p class="text-gray-500 dark:text-gray-400">Cargando cuotas...</p>
        </div>

        <div v-else-if="installments.length === 0" class="text-center py-8">
          <p class="text-gray-500 dark:text-gray-400">No hay cuotas generadas para este préstamo.</p>
        </div>

        <div v-else class="overflow-x-auto">
          <table class="w-full border-collapse">
            <thead>
              <tr class="bg-gray-100 dark:bg-gray-700 border-b border-gray-200 dark:border-gray-600">
                <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">Cuota #</th>
                <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">Fecha Esperada</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900 dark:text-gray-100">Monto Esperado</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900 dark:text-gray-100">Pagado</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900 dark:text-gray-100">Saldo</th>
                <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">Estado</th>
                <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">Acciones</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="installment in installments" :key="installment.id"
                class="border-b border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50">
                <td class="px-4 py-3 text-sm font-medium text-gray-700 dark:text-gray-300">{{ installment.periodNumber }}</td>
                <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ formatDate(installment.expectedPaymentDate) }}</td>
                <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300 text-right">${{ formatCurrency(installment.expectedAmount) }}
                </td>
                <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300 text-right">${{ formatCurrency(installment.paidAmount) }}</td>
                <td class="px-4 py-3 text-sm text-right" :class="getBalanceClass(installment)">
                  ${{ formatCurrency(getBalance(installment)) }}
                </td>
                <td class="px-4 py-3 text-center">
                  <span :class="getStatusClass(installment)" class="px-2 py-1 rounded-full text-xs font-medium">
                    {{ getStatusText(installment.status) }}
                  </span>
                </td>
                <td class="px-4 py-3 text-center">
                  <button v-if="installment.status !== 2 && canPayInstallment(installment)"
                    @click="openPaymentModal(installment)"
                    class="bg-emerald-600 hover:bg-emerald-700 dark:bg-emerald-500 dark:hover:bg-emerald-600 text-white px-3 py-1 rounded text-sm font-medium transition-colors">
                    Pagar
                  </button>
                  <button v-else-if="installment.status !== 2 && !canPayInstallment(installment)" disabled
                    class="bg-gray-300 dark:bg-gray-600 text-gray-500 dark:text-gray-400 px-3 py-1 rounded text-sm font-medium cursor-not-allowed"
                    title="Debe completar las cuotas anteriores primero">
                    Pagar
                  </button>
                  <span v-else class="text-gray-400 dark:text-gray-500 text-sm">Pagado</span>
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr class="bg-gray-50 dark:bg-gray-700 font-semibold">
                <td colspan="2" class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300">Totales</td>
                <td class="px-4 py-3 text-sm text-gray-900 dark:text-gray-100 text-right">${{ formatCurrency(totalExpected) }}</td>
                <td class="px-4 py-3 text-sm text-gray-900 dark:text-gray-100 text-right">${{ formatCurrency(totalPaid) }}</td>
                <td class="px-4 py-3 text-sm text-gray-900 dark:text-gray-100 text-right">${{ formatCurrency(totalBalance) }}</td>
                <td colspan="2"></td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>

      <!-- Footer -->
      <div class="px-6 py-4 border-t border-gray-200 dark:border-gray-700 flex justify-end">
        <button @click="close"
          class="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md text-sm font-medium text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
          Cerrar
        </button>
      </div>
    </div>

    <!-- Payment Modal -->
    <PaymentModal ref="paymentModal" @done="handlePaymentDone" />
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { getInstallmentsByLoan } from '@/services/Installment/installments'
import PaymentModal from './PaymentModal.vue'
import { formatCurrency } from '@/utils/formatters'

const emit = defineEmits(['paymentDone'])

const isOpen = ref(false)
const loading = ref(false)
const installments = ref([])
const loan = ref(null)
const paymentModal = ref(null)

const totalExpected = computed(() => {
  return installments.value.reduce((sum, inst) => sum + inst.expectedAmount, 0)
})

const totalPaid = computed(() => {
  return installments.value.reduce((sum, inst) => sum + inst.paidAmount, 0)
})

const totalBalance = computed(() => {
  return installments.value.reduce((sum, inst) => sum + getBalance(inst), 0)
})

const open = async (loanData) => {
  loan.value = loanData
  isOpen.value = true
  loading.value = true

  try {
    installments.value = await getInstallmentsByLoan(loanData.id)
  } catch (error) {
    alert('Error al cargar las cuotas')
  } finally {
    loading.value = false
  }
}

const close = () => {
  isOpen.value = false
  installments.value = []
  loan.value = null
}

const openPaymentModal = (installment) => {
  paymentModal.value?.open(installment, loan.value)
}

const handlePaymentDone = async () => {
  // Reload installments after payment
  if (loan.value) {
    loading.value = true
    try {
      installments.value = await getInstallmentsByLoan(loan.value.id)
      emit('paymentDone')
    } catch (error) {
      // Error handled silently
    } finally {
      loading.value = false
    }
  }
}

const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('es-ES', { year: 'numeric', month: 'long', day: 'numeric' })
}

const getBalance = (installment) => {
  return installment.expectedAmount - installment.paidAmount
}

const getBalanceClass = (installment) => {
  const balance = getBalance(installment)
  if (balance === 0) return 'text-emerald-600 dark:text-emerald-400 font-semibold'
  if (balance > 0) return 'text-red-600 dark:text-red-400 font-semibold'
  return 'text-gray-700 dark:text-gray-300'
}

const getStatusText = (status) => {
  const statusMap = {
    0: 'Pendiente',
    1: 'Parcial',
    2: 'Pagado'
  }
  return statusMap[status] || 'Desconocido'
}

const getStatusClass = (installment) => {
  const statusClasses = {
    0: 'bg-orange-100 text-orange-800 dark:bg-orange-900/30 dark:text-orange-300',
    1: 'bg-sky-100 text-sky-800 dark:bg-sky-900/30 dark:text-sky-300',
    2: 'bg-emerald-100 text-emerald-800 dark:bg-emerald-900/30 dark:text-emerald-300'
  }
  return statusClasses[installment.status] || 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
}

const canPayInstallment = (installment) => {
  // Si es la primera cuota, siempre se puede pagar
  if (installment.periodNumber === 1) return true

  // Verificar que todas las cuotas anteriores estén pagadas
  return !installments.value.some(
    inst => inst.periodNumber < installment.periodNumber && inst.status !== 2
  )
}

defineExpose({ open, close })
</script>
