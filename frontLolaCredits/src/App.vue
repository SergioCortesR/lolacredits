<script setup>
import { onMounted } from 'vue'
import AppNavBar from './components/System/AppNavBar.vue'
import { useTheme } from './composables/useTheme'

const { initTheme } = useTheme()

onMounted(() => {
  initTheme()
})
</script>

<template>
  <div id="app" class="min-h-screen bg-gray-50 dark:bg-gray-900 text-gray-800 dark:text-gray-100 flex flex-col transition-colors duration-200">
    <!-- Top navigation -->
    <AppNavBar />

    <!-- Main layout: optional sidebar + content -->
    <div class="flex-1 container mx-auto px-4 py-6 w-full">
      <div class="flex flex-col md:flex-row gap-6">
        <!-- Sidebar (optional, hidden on small screens) -->
        <aside class="hidden md:block w-64">
          <div class="sticky top-6 space-y-4">
            <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-4">
              <h3 class="text-sm font-semibold text-gray-700 dark:text-gray-200">Acciones rápidas</h3>
              <ul class="mt-3 space-y-2 text-sm text-gray-600 dark:text-gray-400">
                <li><RouterLink to="/" class="hover:text-sky-600 dark:hover:text-sky-400 transition-colors">Dashboard</RouterLink></li>
                <li><RouterLink to="/persons" class="hover:text-sky-600 dark:hover:text-sky-400 transition-colors">Clientes</RouterLink></li>
                <li><RouterLink to="/loans" class="hover:text-sky-600 dark:hover:text-sky-400 transition-colors">Préstamos</RouterLink></li>
              </ul>
            </div>
          </div>
        </aside>

        <!-- Main content -->
        <main class="flex-1">
          <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 p-6 min-h-[60vh]">
            <RouterView />
          </div>
        </main>
      </div>
    </div>

    <!-- Footer -->
    <footer class="border-t border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800">
      <div class="container mx-auto px-4 py-4 text-sm text-gray-500 dark:text-gray-400">© {{ new Date().getFullYear() }} LolaCredits</div>
    </footer>
  </div>
</template>