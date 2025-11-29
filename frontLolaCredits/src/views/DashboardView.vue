<template>
  <div class="space-y-6">
    <div>
      <h1 class="text-2xl font-bold text-gray-900 dark:text-gray-100">Dashboard</h1>
      <p class="text-gray-600 dark:text-gray-400 mt-1">
        Monitorea el estado financiero de tu negocio en tiempo real. Visualiza métricas clave de clientes, 
        préstamos activos y el flujo de pagos para tomar decisiones informadas.
      </p>
    </div>

    <div v-if="loading" class="text-center py-12">
      <p class="text-gray-500 dark:text-gray-400">Cargando estadísticas...</p>
    </div>

    <div v-else-if="error" class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <!-- Total Clients -->
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 transition-all hover:shadow-md">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Total Clientes</p>
            <p class="text-3xl font-bold text-gray-900 dark:text-gray-100 mt-2">{{ formatNumber(stats.totalPersons) }}</p>
          </div>
          <div class="bg-sky-100 dark:bg-sky-900/30 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-sky-600 dark:text-sky-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Total Loans -->
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 transition-all hover:shadow-md">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Total Préstamos</p>
            <p class="text-3xl font-bold text-gray-900 dark:text-gray-100 mt-2">{{ formatNumber(stats.totalLoans) }}</p>
          </div>
          <div class="bg-indigo-100 dark:bg-indigo-900/30 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-indigo-600 dark:text-indigo-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Active Loans -->
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 transition-all hover:shadow-md">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Préstamos Activos</p>
            <p class="text-3xl font-bold text-emerald-600 dark:text-emerald-400 mt-2">{{ formatNumber(stats.activeLoans) }}</p>
          </div>
          <div class="bg-emerald-100 dark:bg-emerald-900/30 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-emerald-600 dark:text-emerald-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Completed Loans -->
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 transition-all hover:shadow-md">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Préstamos Completados</p>
            <p class="text-3xl font-bold text-gray-900 dark:text-gray-100 mt-2">{{ formatNumber(stats.completedLoans) }}</p>
          </div>
          <div class="bg-gray-100 dark:bg-gray-700 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-gray-600 dark:text-gray-300" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M5 13l4 4L19 7" />
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Financial Summary -->
    <div v-if="!loading && !error" class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <!-- Total Loaned -->
      <div class="bg-linear-to-b from-sky-50 to-sky-100 dark:from-sky-900/20 dark:to-sky-800/20 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-sky-200 dark:border-sky-800 p-6 transition-all hover:shadow-md">
        <p class="text-sm font-medium text-sky-700 dark:text-sky-400 mb-2">Total Prestado</p>
        <p class="text-2xl font-bold text-sky-900 dark:text-sky-300">${{ formatCurrency(stats.totalLoaned) }}</p>
        <p class="text-xs text-sky-600 dark:text-sky-500 mt-2">Monto total de todos los préstamos</p>
      </div>

      <!-- Total Paid -->
      <div class="bg-linear-to-b from-emerald-50 to-emerald-100 dark:from-emerald-900/20 dark:to-emerald-800/20 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-emerald-200 dark:border-emerald-800 p-6 transition-all hover:shadow-md">
        <p class="text-sm font-medium text-emerald-700 dark:text-emerald-400 mb-2">Total Cobrado</p>
        <p class="text-2xl font-bold text-emerald-900 dark:text-emerald-300">${{ formatCurrency(stats.totalPaid) }}</p>
        <p class="text-xs text-emerald-600 dark:text-emerald-500 mt-2">Pagos recibidos acumulados</p>
      </div>

      <!-- Total Pending -->
      <div class="bg-linear-to-b from-orange-50 to-orange-100 dark:from-orange-900/20 dark:to-orange-800/20 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-orange-200 dark:border-orange-800 p-6 transition-all hover:shadow-md">
        <p class="text-sm font-medium text-orange-700 dark:text-orange-400 mb-2">Total Pendiente</p>
        <p class="text-2xl font-bold text-orange-900 dark:text-orange-300">${{ formatCurrency(stats.totalPending) }}</p>
        <p class="text-xs text-orange-600 dark:text-orange-500 mt-2">Saldo por cobrar</p>
      </div>
    </div>

    <!-- Quick Actions -->
    <div v-if="!loading && !error" class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6">
      <h2 class="text-lg font-semibold text-gray-900 dark:text-gray-100 mb-4">Acciones Rápidas</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <router-link to="/persons"
          class="flex items-center gap-3 p-4 border border-gray-200 dark:border-gray-700 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors">
          <div class="bg-sky-100 dark:bg-sky-900/30 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-sky-600 dark:text-sky-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900 dark:text-gray-100">Gestionar Clientes</p>
            <p class="text-sm text-gray-500 dark:text-gray-400">Ver y editar clientes</p>
          </div>
        </router-link>

        <router-link to="/loans"
          class="flex items-center gap-3 p-4 border border-gray-200 dark:border-gray-700 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors">
          <div class="bg-indigo-100 dark:bg-indigo-900/30 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-indigo-600 dark:text-indigo-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900 dark:text-gray-100">Gestionar Préstamos</p>
            <p class="text-sm text-gray-500 dark:text-gray-400">Ver y crear préstamos</p>
          </div>
        </router-link>

        <button @click="loadStats"
          class="flex items-center gap-3 p-4 border border-gray-200 dark:border-gray-700 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors">
          <div class="bg-emerald-100 dark:bg-emerald-900/30 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-emerald-600 dark:text-emerald-400" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900 dark:text-gray-100">Actualizar Datos</p>
            <p class="text-sm text-gray-500 dark:text-gray-400">Refrescar estadísticas</p>
          </div>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getDashboardStats } from '@/services/Dashboard/dashboard'
import { formatCurrency, formatNumber } from '@/utils/formatters'

const stats = ref({
  totalPersons: 0,
  totalLoans: 0,
  activeLoans: 0,
  completedLoans: 0,
  totalLoaned: 0,
  totalPaid: 0,
  totalPending: 0
})
const loading = ref(false)
const error = ref('')

const loadStats = async () => {
  loading.value = true
  error.value = ''

  try {
    stats.value = await getDashboardStats()
  } catch (err) {
    error.value = 'Error al cargar las estadísticas. Por favor, intente nuevamente.'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadStats()
})
</script>