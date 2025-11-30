<template>
  <header class="bg-white dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700 transition-colors">
    <div class="container mx-auto px-4 py-3 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <RouterLink to="/" class="flex items-center gap-2">
          <img src="@/assets/logo.png" alt="logo" class="h-9 w-9" />
          <span class="text-xl font-bold text-sky-600 dark:text-sky-400">LolaCredits</span>
        </RouterLink>
      </div>
      <nav class="hidden md:flex items-center gap-6 text-sm font-medium">
        <div class="text-gray-600 dark:text-gray-400">
          <div>Hecho por Sergio Cortes</div>
          <div>
            <a :href="swaggerUrl" target="_blank" class="text-sky-600 dark:text-sky-400 hover:text-sky-700 dark:hover:text-sky-300 transition-colors">
              Documentación en Swagger
            </a>
          </div>
        </div>
        <!-- Dark mode toggle -->
        <button 
          @click="toggleTheme" 
          class="p-2 rounded-lg bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors"
          :title="isDark ? 'Modo claro' : 'Modo oscuro'"
        >
          <!-- Sun icon (light mode) -->
          <svg v-if="isDark" xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-yellow-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
          </svg>
          <!-- Moon icon (dark mode) -->
          <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-600 dark:text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" />
          </svg>
        </button>
      </nav>
      <!-- Mobile menu button & dark mode toggle -->
      <div class="md:hidden flex items-center gap-3">
        <button 
          @click="toggleTheme" 
          class="p-3 rounded-lg bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors active:scale-95"
          :title="isDark ? 'Modo claro' : 'Modo oscuro'"
        >
          <svg v-if="isDark" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-yellow-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
          </svg>
          <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" />
          </svg>
        </button>
        <button 
          @click="toggleMenu" 
          class="p-3 rounded-lg bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors active:scale-95"
          :title="open ? 'Cerrar menú' : 'Abrir menú'"
        >
          <svg v-if="!open" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-gray-600 dark:text-gray-300" fill="none" viewBox="0 0 24 24"
            stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
          <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-gray-600 dark:text-gray-300" fill="none" viewBox="0 0 24 24"
            stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </div>

    <!-- Mobile menu -->
    <div v-if="open" class="md:hidden border-t border-gray-100 dark:border-gray-700 bg-white dark:bg-gray-800">
      <div class="px-4 py-4 space-y-1">
        <RouterLink 
          @click="toggleMenu"
          to="/" 
          class="block px-4 py-3 text-base text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 hover:text-sky-600 dark:hover:text-sky-400 font-medium transition-colors rounded-lg"
        >
          Dashboard
        </RouterLink>
        <RouterLink 
          @click="toggleMenu"
          to="/persons" 
          class="block px-4 py-3 text-base text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 hover:text-sky-600 dark:hover:text-sky-400 font-medium transition-colors rounded-lg"
        >
          Clientes
        </RouterLink>
        <RouterLink 
          @click="toggleMenu"
          to="/loans" 
          class="block px-4 py-3 text-base text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 hover:text-sky-600 dark:hover:text-sky-400 font-medium transition-colors rounded-lg"
        >
          Préstamos
        </RouterLink>
      </div>
    </div>
  </header>
</template>
<script setup>
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
import { useTheme } from '@/composables/useTheme'

const open = ref(false)
const { isDark, toggleTheme } = useTheme()

const toggleMenu = () => {
  open.value = !open.value
}
const swaggerUrl = `${import.meta.env.VITE_API_BASE_URL}swagger/index.html`;
</script>
