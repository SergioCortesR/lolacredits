<script setup>
import AppNavBar from './components/System/AppNavBar.vue'
</script>

<template>
  <div id="app" class="min-h-screen bg-gray-50 text-gray-800 flex flex-col">
    <!-- Top navigation -->
    <AppNavBar />

    <!-- Main layout: optional sidebar + content -->
    <div class="flex-1 container mx-auto px-4 py-6 w-full">
      <div class="flex flex-col md:flex-row gap-6">
        <!-- Sidebar (optional, hidden on small screens) -->
        <aside class="hidden md:block w-64">
          <div class="sticky top-6 space-y-4">
            <div class="bg-white rounded-lg shadow border border-gray-200 p-4">
              <h3 class="text-sm font-semibold text-gray-700">Aciones rápidas</h3>
              <ul class="mt-3 space-y-2 text-sm text-gray-600">
                <li><RouterLink to="/" class="hover:text-gray-900">Dashboard</RouterLink></li>
                <li><RouterLink to="/persons" class="hover:text-gray-900">Personas</RouterLink></li>
                <li><RouterLink to="/loans" class="hover:text-gray-900">Prestamos</RouterLink></li>
              </ul>
            </div>
          </div>
        </aside>

        <!-- Main content -->
        <main class="flex-1">
          <div class="bg-white rounded-lg shadow border border-gray-200 p-6 min-h-[60vh]">
            <RouterView />
          </div>
        </main>
      </div>
    </div>

    <!-- Footer -->
    <footer class="border-t border-gray-200 bg-white">
      <div class="container mx-auto px-4 py-4 text-sm text-gray-500">© {{ new Date().getFullYear() }} LolaCredits</div>
    </footer>
  </div>
</template>