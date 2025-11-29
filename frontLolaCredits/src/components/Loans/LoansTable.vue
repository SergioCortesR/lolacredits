<template>
  <div class="space-y-4">
    <!-- Error message -->
    <div v-if="error" class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <!-- Header with search and Create button -->
    <div class="flex flex-col md:flex-row gap-4 justify-between items-start md:items-center">
      <div class="flex-1 w-full md:w-auto">
        <input v-model="filters.search" @input="debouncedSearch" type="text"
          placeholder="Buscar por cliente, CI, email..."
          class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-lg focus:outline-none focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400" />
      </div>
      <button @click="handleCreate"
        class="bg-sky-600 hover:bg-sky-700 dark:bg-sky-500 dark:hover:bg-sky-600 text-white font-medium py-2 px-4 rounded-lg transition-colors whitespace-nowrap">
        + Nuevo Préstamo
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="text-center text-gray-500 dark:text-gray-400 py-8">Cargando...</div>

    <!-- Empty state -->
    <div v-else-if="!loans.items || loans.items.length === 0"
      class="bg-white dark:bg-gray-800 p-6 rounded-lg border border-gray-200 dark:border-gray-700 text-center text-gray-500 dark:text-gray-400">
      No hay préstamos aún. Crea uno para comenzar.
    </div>

    <!-- Table -->
    <div v-else class="space-y-4">
      <div class="overflow-x-auto">
        <table class="w-full border-collapse">
          <ConfirmDialog ref="confirmDialog" />
          <thead>
            <tr class="bg-gray-100 dark:bg-gray-700 border-b border-gray-200 dark:border-gray-600">
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">Cliente</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">CI</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">Email</th>
              <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900 dark:text-gray-100">
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
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">
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
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">
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
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">
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
              <th class="px-4 py-3 text-right text-sm font-semibold text-gray-900 dark:text-gray-100">Cuota Mensual</th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">Estado</th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="loan in loans.items" :key="loan.id" class="border-b border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50">
              <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300">{{ loan.personName }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ loan.personCI }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ loan.personEmail }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300 text-right">${{ formatCurrency(loan.amount) }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400 text-center">{{ loan.months }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400 text-center">{{ loan.interestRate }}%</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ formatDate(loan.loanDate) }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300 text-right">${{ formatCurrency(loan.monthlyAmount) }}</td>
              <td class="px-4 py-3 text-center">
                <span :class="getStatusClass(loan)" class="px-2 py-1 rounded-full text-xs font-medium">
                  {{ getStatusText(loan) }}
                </span>
              </td>
              <td class="px-4 py-3 text-center space-x-2">
                <button @click="handleViewInstallments(loan)"
                  class="text-indigo-600 dark:text-indigo-400 hover:text-indigo-900 dark:hover:text-indigo-300 font-medium text-sm inline-flex items-center"
                  title="Ver cuotas">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="M2.25 18.75a60.07 60.07 0 0 1 15.797 2.101c.727.198 1.453-.342 1.453-1.096V18.75M3.75 4.5v.75A.75.75 0 0 1 3 6h-.75m0 0v-.375c0-.621.504-1.125 1.125-1.125H20.25M2.25 6v9m18-10.5v.75c0 .414.336.75.75.75h.75m-1.5-1.5h.375c.621 0 1.125.504 1.125 1.125v9.75c0 .621-.504 1.125-1.125 1.125h-.375m1.5-1.5H21a.75.75 0 0 0-.75.75v.75m0 0H3.75m0 0h-.375a1.125 1.125 0 0 1-1.125-1.125V15m1.5 1.5v-.75A.75.75 0 0 0 3 15h-.75M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Zm3 0h.008v.008H18V10.5Zm-12 0h.008v.008H6V10.5Z" />
                  </svg>
                </button>
                <button @click="handleEdit(loan)" class="text-sky-600 dark:text-sky-400 hover:text-sky-900 dark:hover:text-sky-300 font-medium text-sm"
                  title="Editar">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
                  </svg>
                </button>
                <button @click="handleDelete(loan.id)" class="text-red-600 dark:text-red-400 hover:text-red-900 dark:hover:text-red-300 font-medium text-sm"
                  title="Eliminar">
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
      <div class="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-gray-200 dark:border-gray-700">
        <div class="text-sm text-gray-600 dark:text-gray-400">
          Mostrando del {{ (loans.page - 1) * loans.pageSize + 1 }} al {{ Math.min(loans.page * loans.pageSize,
            loans.totalItems) }} de {{ loans.totalItems }} resultados
        </div>
        <div class="flex items-center gap-2">
          <button @click="changePage(loans.page - 1)" :disabled="loans.page === 1"
            class="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300">
            Anterior
          </button>
          <div class="flex gap-1">
            <button v-for="page in visiblePages" :key="page" @click="changePage(page)" :class="[
              'px-3 py-1 border rounded-lg transition-colors',
              page === loans.page
                ? 'bg-sky-600 dark:bg-sky-500 text-white border-sky-600 dark:border-sky-500'
                : 'border-gray-300 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 text-gray-700 dark:text-gray-300'
            ]">
              {{ page }}
            </button>
          </div>
          <button @click="changePage(loans.page + 1)" :disabled="loans.page >= totalPages"
            class="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300">
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

const emit = defineEmits(['edit', 'create', 'viewInstallments'])

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
    error.value = 'Error al cargar préstamos'
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

const handleViewInstallments = (loan) => {
  emit('viewInstallments', loan)
}
</script>
