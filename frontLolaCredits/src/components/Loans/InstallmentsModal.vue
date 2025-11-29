<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl w-full max-w-5xl max-h-[90vh] overflow-hidden">
      <!-- Header -->
      <div class="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
        <div>
          <h2 class="text-xl font-semibold text-gray-800">Cuotas del Préstamo</h2>
          <p v-if="loan" class="text-sm text-gray-600 mt-1">
            Cliente: {{ loan.personName }} | Monto: ${{ loan.amount.toFixed(2) }} | {{ loan.months }} meses
          </p>
        </div>
        <button @click="close" class="text-gray-400 hover:text-gray-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Content -->
      <div class="p-6 overflow-y-auto max-h-[calc(90vh-140px)]">
        <div v-if="loading" class="text-center py-8">
          <p class="text-gray-500">Cargando cuotas...</p>
        </div>

        <div v-else-if="installments.length === 0" class="text-center py-8">
          <p class="text-gray-500">No hay cuotas generadas para este préstamo.</p>
        </div>

        <div v-else class="overflow-x-auto">
          <table class="w-full border-collapse">
            <thead>
              <tr class="bg-gray-100 border-b border-gray-200">
                <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Cuota #</th>
                <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Fecha Esperada</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900">Monto Esperado</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900">Pagado</th>
                <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900">Saldo</th>
                <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Estado</th>
                <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Acciones</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="installment in installments" :key="installment.id"
                class="border-b border-gray-200 hover:bg-gray-50">
                <td class="px-4 py-3 text-sm font-medium text-gray-700">{{ installment.periodNumber }}</td>
                <td class="px-4 py-3 text-sm text-gray-600">{{ formatDate(installment.expectedPaymentDate) }}</td>
                <td class="px-4 py-3 text-sm text-gray-700 text-right">${{ installment.expectedAmount.toFixed(2) }}
                </td>
                <td class="px-4 py-3 text-sm text-gray-700 text-right">${{ installment.paidAmount.toFixed(2) }}</td>
                <td class="px-4 py-3 text-sm text-right" :class="getBalanceClass(installment)">
                  ${{ getBalance(installment).toFixed(2) }}
                </td>
                <td class="px-4 py-3 text-center">
                  <span :class="getStatusClass(installment)" class="px-2 py-1 rounded-full text-xs font-medium">
                    {{ getStatusText(installment.status) }}
                  </span>
                </td>
                <td class="px-4 py-3 text-center">
                  <button v-if="installment.status !== 2 && canPayInstallment(installment)"
                    @click="openPaymentModal(installment)"
                    class="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-sm font-medium">
                    Pagar
                  </button>
                  <button v-else-if="installment.status !== 2 && !canPayInstallment(installment)" disabled
                    class="bg-gray-300 text-gray-500 px-3 py-1 rounded text-sm font-medium cursor-not-allowed"
                    title="Debe completar las cuotas anteriores primero">
                    Pagar
                  </button>
                  <span v-else class="text-gray-400 text-sm">Pagado</span>
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr class="bg-gray-50 font-semibold">
                <td colspan="2" class="px-4 py-3 text-sm text-gray-700">Totales</td>
                <td class="px-4 py-3 text-sm text-gray-900 text-right">${{ totalExpected.toFixed(2) }}</td>
                <td class="px-4 py-3 text-sm text-gray-900 text-right">${{ totalPaid.toFixed(2) }}</td>
                <td class="px-4 py-3 text-sm text-gray-900 text-right">${{ totalBalance.toFixed(2) }}</td>
                <td colspan="2"></td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>

      <!-- Footer -->
      <div class="px-6 py-4 border-t border-gray-200 flex justify-end">
        <button @click="close"
          class="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-50">
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
    console.error('Error loading installments:', error)
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
      console.error('Error reloading installments:', error)
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
  if (balance === 0) return 'text-green-600 font-semibold'
  if (balance > 0) return 'text-red-600 font-semibold'
  return 'text-gray-700'
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
    0: 'bg-yellow-100 text-yellow-800',
    1: 'bg-blue-100 text-blue-800',
    2: 'bg-green-100 text-green-800'
  }
  return statusClasses[installment.status] || 'bg-gray-100 text-gray-800'
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
