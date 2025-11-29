<template>
  <header class="bg-white border-b border-gray-200">
    <div class="container mx-auto px-4 py-3 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <RouterLink to="/" class="flex items-center gap-2">
          <img src="@/assets/logo.png" alt="logo" class="h-9 w-9" />
          <span class="text-xl font-bold text-blue-600">LolaCredits</span>
        </RouterLink>
      </div>
      <nav class="hidden md:flex items-center gap-6 text-sm font-medium">
        <div>
          <div>Hecho por Sergio Cortes</div>
          <div>
            <a :href="swaggerUrl" target="_blank" class="text-blue-600">
              Documentación en Swagger
            </a>
          </div>
        </div>
      </nav>
      <!-- Mobile menu button -->
      <div class="md:hidden">
        <button @click="toggleMenu" class="p-2 rounded-md bg-gray-100 hover:bg-gray-200">
          <svg v-if="!open" xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24"
            stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
          <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24"
            stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </div>

    <!-- Mobile menu -->
    <div v-if="open" class="md:hidden border-t border-gray-100 bg-white">
      <div class="px-4 py-3 space-y-2">
        <RouterLink to="/" class="block text-gray-600 hover:text-gray-900 font-medium transition-colors">
          Dashboard
        </RouterLink>
        <RouterLink to="/persons" class="block text-gray-600 hover:text-gray-900 font-medium transition-colors">
          Clientes
        </RouterLink>
        <RouterLink to="/loans" class="block text-gray-600 hover:text-gray-900 font-medium transition-colors">
          Préstamos
        </RouterLink>
      </div>
    </div>
  </header>
</template>
<script setup>
import { ref } from 'vue'
import { RouterLink } from 'vue-router'

const open = ref(false)

const toggleMenu = () => {
  open.value = !open.value
}
const swaggerUrl = `${import.meta.env.VITE_API_BASE_URL}swagger/index.html`;
</script>
