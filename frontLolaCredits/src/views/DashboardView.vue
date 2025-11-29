<template>
  <div class="space-y-6">
    <div>
      <h1 class="text-2xl font-bold text-gray-900">Dashboard</h1>
      <p class="text-gray-600 mt-1">Resumen general de clientes y préstamos</p>
    </div>

    <div v-if="loading" class="text-center py-12">
      <p class="text-gray-500">Cargando estadísticas...</p>
    </div>

    <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <!-- Total Clients -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Total Clientes</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ stats.totalPersons }}</p>
          </div>
          <div class="bg-blue-100 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-blue-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Total Loans -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Total Préstamos</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ stats.totalLoans }}</p>
          </div>
          <div class="bg-purple-100 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-purple-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Active Loans -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Préstamos Activos</p>
            <p class="text-3xl font-bold text-green-600 mt-2">{{ stats.activeLoans }}</p>
          </div>
          <div class="bg-green-100 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-green-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Completed Loans -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Préstamos Completados</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ stats.completedLoans }}</p>
          </div>
          <div class="bg-gray-100 rounded-full p-3">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-gray-600" fill="none" viewBox="0 0 24 24"
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
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-600 mb-2">Total Prestado</p>
        <p class="text-2xl font-bold text-blue-600">${{ stats.totalLoaned.toFixed(2) }}</p>
        <p class="text-xs text-gray-500 mt-2">Monto total de todos los préstamos</p>
      </div>

      <!-- Total Paid -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-600 mb-2">Total Cobrado</p>
        <p class="text-2xl font-bold text-green-600">${{ stats.totalPaid.toFixed(2) }}</p>
        <p class="text-xs text-gray-500 mt-2">Pagos recibidos acumulados</p>
      </div>

      <!-- Total Pending -->
      <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-600 mb-2">Total Pendiente</p>
        <p class="text-2xl font-bold text-red-600">${{ stats.totalPending.toFixed(2) }}</p>
        <p class="text-xs text-gray-500 mt-2">Saldo por cobrar</p>
      </div>
    </div>

    <!-- Quick Actions -->
    <div v-if="!loading && !error" class="bg-white rounded-lg shadow border border-gray-200 p-6">
      <h2 class="text-lg font-semibold text-gray-900 mb-4">Acciones Rápidas</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <router-link to="/persons"
          class="flex items-center gap-3 p-4 border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors">
          <div class="bg-blue-100 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900">Gestionar Clientes</p>
            <p class="text-sm text-gray-500">Ver y editar clientes</p>
          </div>
        </router-link>

        <router-link to="/loans"
          class="flex items-center gap-3 p-4 border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors">
          <div class="bg-purple-100 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-purple-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900">Gestionar Préstamos</p>
            <p class="text-sm text-gray-500">Ver y crear préstamos</p>
          </div>
        </router-link>

        <button @click="loadStats"
          class="flex items-center gap-3 p-4 border border-gray-200 rounded-lg hover:bg-gray-50 transition-colors">
          <div class="bg-green-100 rounded-full p-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-green-600" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
            </svg>
          </div>
          <div>
            <p class="font-medium text-gray-900">Actualizar Datos</p>
            <p class="text-sm text-gray-500">Refrescar estadísticas</p>
          </div>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getDashboardStats } from '@/services/Dashboard/dashboard'

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
    console.error('Error loading dashboard stats:', err)
    error.value = 'Error al cargar las estadísticas. Por favor, intente nuevamente.'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadStats()
})
</script>