<template>
  <div class="space-y-4">
    <!-- Error message -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <!-- Header with search and Create button -->
    <div class="flex flex-col md:flex-row gap-4 justify-between items-start md:items-center">
      <div class="flex-1 w-full md:w-auto">
        <input v-model="filters.search" @input="debouncedSearch" type="text"
          placeholder="Buscar por cliente, CI, email..."
          class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" />
      </div>
      <button @click="handleCreate"
        class="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition-colors whitespace-nowrap">
        + Nuevo Préstamo
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="text-center text-gray-500 py-8">Loading...</div>

    <!-- Empty state -->
    <div v-else-if="!loans.items || loans.items.length === 0"
      class="bg-white p-6 rounded-lg border border-gray-200 text-center text-gray-500">
      No hay préstamos aún. Crea uno para comenzar.
    </div>

    <!-- Table -->
    <div v-else class="space-y-4">
      <div class="overflow-x-auto">
        <table class="w-full border-collapse">
          <ConfirmDialog ref="confirmDialog" />
          <thead>
            <tr class="bg-gray-100 border-b border-gray-200">
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Cliente</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">CI</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Email</th>
              <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900">
                <div class="flex items-center justify-end gap-2 cursor-pointer" @click="toggleSort('Amount')">
                  Monto
                  <svg v-if="filters.sort === 'Amount'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">
                <div class="flex items-center justify-center gap-2 cursor-pointer" @click="toggleSort('Months')">
                  Meses
                  <svg v-if="filters.sort === 'Months'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">
                <div class="flex items-center justify-center gap-2 cursor-pointer" @click="toggleSort('InterestRate')">
                  Interés
                  <svg v-if="filters.sort === 'InterestRate'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4"
                    fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">
                <div class="flex items-center gap-2 cursor-pointer" @click="toggleSort('LoanDate')">
                  Fecha Préstamo
                  <svg v-if="filters.sort === 'LoanDate'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900">Cuota Mensual</th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Estado</th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="loan in loans.items" :key="loan.id" class="border-b border-gray-200 hover:bg-gray-50">
              <td class="px-4 py-3 text-sm text-gray-700">{{ loan.personName }}</td>
              <td class="px-4 py-3 text-sm text-gray-600">{{ loan.personCI }}</td>
              <td class="px-4 py-3 text-sm text-gray-600">{{ loan.personEmail }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 text-right">${{ loan.amount.toFixed(2) }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 text-center">{{ loan.months }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 text-center">{{ loan.interestRate }}%</td>
              <td class="px-4 py-3 text-sm text-gray-600">{{ formatDate(loan.loanDate) }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 text-right">${{ loan.monthlyAmount.toFixed(2) }}</td>
              <td class="px-4 py-3 text-center">
                <span :class="getStatusClass(loan)" class="px-2 py-1 rounded-full text-xs font-medium">
                  {{ getStatusText(loan) }}
                </span>
              </td>
              <td class="px-4 py-3 text-center space-x-2">
                <button @click="handleEdit(loan)" class="text-blue-600 hover:text-blue-900 font-medium text-sm">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
                  </svg>
                </button>
                <button @click="handleDelete(loan.id)" class="text-red-600 hover:text-red-900 font-medium text-sm">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
                  </svg>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <div class="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-gray-200">
        <div class="text-sm text-gray-600">
          Mostrando del {{ (loans.page - 1) * loans.pageSize + 1 }} al {{ Math.min(loans.page * loans.pageSize,
            loans.totalItems) }} de {{ loans.totalItems }} resultados
        </div>
        <div class="flex items-center gap-2">
          <button @click="changePage(loans.page - 1)" :disabled="loans.page === 1"
            class="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed">
            Anterior
          </button>
          <div class="flex gap-1">
            <button v-for="page in visiblePages" :key="page" @click="changePage(page)" :class="[
              'px-3 py-1 border rounded-lg',
              page === loans.page
                ? 'bg-blue-600 text-white border-blue-600'
                : 'border-gray-300 hover:bg-gray-50'
            ]">
              {{ page }}
            </button>
          </div>
          <button @click="changePage(loans.page + 1)" :disabled="loans.page >= totalPages"
            class="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed">
            Siguiente
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, onMounted, computed } from 'vue'
import { getLoans, deleteLoan } from '@/services/Loan/loans'
import ConfirmDialog from '@/components/Form/ConfirmAction.vue'

const loans = ref({
  items: [],
  totalItems: 0,
  page: 1,
  pageSize: 10
});
const loading = ref(false);
const confirmDialog = ref(null);
const error = ref('');

const filters = ref({
  search: '',
  sort: 'LoanDate',
  dir: 'desc'
})

let debounceTimer = null

const emit = defineEmits(['edit', 'create'])

const totalPages = computed(() => {
  return Math.ceil(loans.value.totalItems / loans.value.pageSize)
})

const visiblePages = computed(() => {
  const current = loans.value.page
  const total = totalPages.value
  const pages = []

  if (total <= 7) {
    for (let i = 1; i <= total; i++) {
      pages.push(i)
    }
  } else {
    if (current <= 4) {
      for (let i = 1; i <= 5; i++) pages.push(i)
      pages.push('...')
      pages.push(total)
    } else if (current >= total - 3) {
      pages.push(1)
      pages.push('...')
      for (let i = total - 4; i <= total; i++) pages.push(i)
    } else {
      pages.push(1)
      pages.push('...')
      for (let i = current - 1; i <= current + 1; i++) pages.push(i)
      pages.push('...')
      pages.push(total)
    }
  }

  return pages.filter(p => p !== '...')
})

onMounted(async () => {
  await loadLoans()
})

const loadLoans = async () => {
  loading.value = true
  error.value = ''
  try {
    const params = {
      page: loans.value.page,
      pageSize: loans.value.pageSize,
      search: filters.value.search || undefined,
      sort: filters.value.sort,
      dir: filters.value.dir
    }
    const response = await getLoans(params)
    loans.value = response.data
  } catch (err) {
    error.value = 'Error loading loans'
    console.error(err)
  } finally {
    loading.value = false
  }
}

const debouncedSearch = () => {
  clearTimeout(debounceTimer)
  debounceTimer = setTimeout(() => {
    loans.value.page = 1
    loadLoans()
  }, 500)
}

const toggleSort = (column) => {
  if (filters.value.sort === column) {
    filters.value.dir = filters.value.dir === 'asc' ? 'desc' : 'asc'
  } else {
    filters.value.sort = column
    filters.value.dir = 'asc'
  }
  loadLoans()
}

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    loans.value.page = page
    loadLoans()
  }
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('es-ES', { year: 'numeric', month: '2-digit', day: '2-digit' })
}

const getStatusText = (loan) => {
  const paid = loan.paidInstallments || 0
  const pending = loan.pendingInstallments || 0
  
  if (pending === 0) return 'Completado'
  if (paid === 0) return 'Pendiente'
  return 'En Progreso'
}

const getStatusClass = (loan) => {
  const paid = loan.paidInstallments || 0
  const pending = loan.pendingInstallments || 0
  
  if (pending === 0) return 'bg-green-100 text-green-800'
  if (paid === 0) return 'bg-yellow-100 text-yellow-800'
  return 'bg-blue-100 text-blue-800'
}

defineExpose({ loadLoans })

const handleDelete = async (id) => {
  const ok = await confirmDialog.value.ask(
    "Eliminar Préstamo",
    "¿Estás seguro de que deseas eliminar este préstamo? Se eliminarán también todas las cuotas y pagos asociados."
  )

  if (!ok) return

  try {
    await deleteLoan(id)
    await loadLoans()
  } catch (err) {
    error.value = "Error deleting loan"
    console.error(err)
  }
}

const handleEdit = (loan) => {
  emit('edit', loan)
}

const handleCreate = () => {
  emit('create')
}
</script>
