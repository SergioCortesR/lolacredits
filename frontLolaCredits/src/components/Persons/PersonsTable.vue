<template>
  <div class="space-y-4">
    <!-- Error message -->
    <div v-if="error" class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <!-- Header with filters and Create button -->
    <div class="flex flex-col md:flex-row gap-4 justify-between items-start md:items-center">
      <div class="flex-1 w-full md:w-auto">
        <input v-model="filters.search" @input="debouncedSearch" type="text" placeholder="Buscar por nombre, CI, email..."
          class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-lg focus:outline-none focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400" />
      </div>
      <button @click="handleCreate"
        class="bg-sky-600 hover:bg-sky-700 dark:bg-sky-500 dark:hover:bg-sky-600 text-white font-medium py-2 px-4 rounded-lg transition-colors whitespace-nowrap">
        + Nuevo Cliente
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="text-center text-gray-500 dark:text-gray-400 py-8">Cargando...</div>

    <!-- Empty state -->
    <div v-else-if="!persons.items || persons.items.length === 0"
      class="bg-white dark:bg-gray-800 p-6 rounded-lg border border-gray-200 dark:border-gray-700 text-center text-gray-500 dark:text-gray-400">
      No hay clientes aún. Crea uno para comenzar.
    </div>

    <!-- Table -->
    <div v-else class="space-y-4">
      <ConfirmDialog ref="confirmDialog" />
      <!-- Desktop Table -->
      <div class="hidden md:block overflow-x-auto">
        <table class="w-full border-collapse">
          <thead>
            <tr class="bg-gray-100 dark:bg-gray-700 border-b border-gray-200 dark:border-gray-600">
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">
                <div class="flex items-center gap-2 cursor-pointer" @click="toggleSort('CI')">
                  CI
                  <svg v-if="filters.sort === 'CI'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">
                <div class="flex items-center gap-2 cursor-pointer" @click="toggleSort('Name')">
                  Name
                  <svg v-if="filters.sort === 'Name'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">Phone</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900 dark:text-gray-100">
                <div class="flex items-center gap-2 cursor-pointer" @click="toggleSort('Email')">
                  Email
                  <svg v-if="filters.sort === 'Email'" xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none"
                    viewBox="0 0 24 24" stroke="currentColor">
                    <path v-if="filters.dir === 'asc'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M5 15l7-7 7 7" />
                    <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </th>
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900 dark:text-gray-100">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="person in persons.items" :key="person.id" class="border-b border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50">
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ person.ci }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300">{{ person.fullName }}</td>
              <td class="px-4 py-3 text-sm text-gray-600 dark:text-gray-400">{{ person.phone ?? 'N/A' }}</td>
              <td class="px-4 py-3 text-sm text-gray-700 dark:text-gray-300">{{ person.email }}</td>
              <td class="px-4 py-3 text-center space-x-2">
                <button @click="handleEdit(person)" class="text-sky-600 dark:text-sky-400 hover:text-sky-900 dark:hover:text-sky-300 font-medium text-sm">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
                  </svg>
                </button>
                <button @click="handleDelete(person.id)" class="text-red-600 dark:text-red-400 hover:text-red-900 dark:hover:text-red-300 font-medium text-sm">
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

      <!-- Mobile Cards -->
      <div class="md:hidden space-y-4">
        <div v-for="person in persons.items" :key="person.id"
          class="bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-lg p-4 space-y-3">
          <div class="space-y-2">
            <h3 class="font-semibold text-lg text-gray-900 dark:text-gray-100">{{ person.fullName }}</h3>
            <div class="space-y-1 text-sm">
              <div class="flex items-center gap-2 text-gray-600 dark:text-gray-400">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M15 9h3.75M15 12h3.75M15 15h3.75M4.5 19.5h15a2.25 2.25 0 0 0 2.25-2.25V6.75A2.25 2.25 0 0 0 19.5 4.5h-15a2.25 2.25 0 0 0-2.25 2.25v10.5A2.25 2.25 0 0 0 4.5 19.5Zm6-10.125a1.875 1.875 0 1 1-3.75 0 1.875 1.875 0 0 1 3.75 0Zm1.294 6.336a6.721 6.721 0 0 1-3.17.789 6.721 6.721 0 0 1-3.168-.789 3.376 3.376 0 0 1 6.338 0Z" />
                </svg>
                <span>CI: {{ person.ci }}</span>
              </div>
              <div class="flex items-center gap-2 text-gray-600 dark:text-gray-400">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M21.75 6.75v10.5a2.25 2.25 0 0 1-2.25 2.25h-15a2.25 2.25 0 0 1-2.25-2.25V6.75m19.5 0A2.25 2.25 0 0 0 19.5 4.5h-15a2.25 2.25 0 0 0-2.25 2.25m19.5 0v.243a2.25 2.25 0 0 1-1.07 1.916l-7.5 4.615a2.25 2.25 0 0 1-2.36 0L3.32 8.91a2.25 2.25 0 0 1-1.07-1.916V6.75" />
                </svg>
                <span>{{ person.email }}</span>
              </div>
              <div class="flex items-center gap-2 text-gray-600 dark:text-gray-400">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M2.25 6.75c0 8.284 6.716 15 15 15h2.25a2.25 2.25 0 0 0 2.25-2.25v-1.372c0-.516-.351-.966-.852-1.091l-4.423-1.106c-.44-.11-.902.055-1.173.417l-.97 1.293c-.282.376-.769.542-1.21.38a12.035 12.035 0 0 1-7.143-7.143c-.162-.441.004-.928.38-1.21l1.293-.97c.363-.271.527-.734.417-1.173L6.963 3.102a1.125 1.125 0 0 0-1.091-.852H4.5A2.25 2.25 0 0 0 2.25 4.5v2.25Z" />
                </svg>
                <span>{{ person.phone ?? 'N/A' }}</span>
              </div>
            </div>
          </div>

          <div class="flex gap-2 pt-2 border-t border-gray-200 dark:border-gray-700">
            <button @click="handleEdit(person)"
              class="flex-1 px-3 py-2 bg-sky-50 dark:bg-sky-900/20 text-sky-600 dark:text-sky-400 rounded-lg text-sm font-medium hover:bg-sky-100 dark:hover:bg-sky-900/40 transition-colors flex items-center justify-center gap-1">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                <path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
              </svg>
              Editar
            </button>
            <button @click="handleDelete(person.id)"
              class="px-3 py-2 bg-red-50 dark:bg-red-900/20 text-red-600 dark:text-red-400 rounded-lg text-sm font-medium hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
              </svg>
            </button>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div class="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t border-gray-200 dark:border-gray-700">
        <div class="text-sm text-gray-600 dark:text-gray-400">
          Mostrando del {{ (persons.page - 1) * persons.pageSize + 1 }} al {{ Math.min(persons.page * persons.pageSize,
            persons.totalItems) }} de {{ persons.totalItems }} resultados
        </div>
        <div class="flex items-center gap-2">
          <button @click="changePage(persons.page - 1)" :disabled="persons.page === 1"
            class="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300">
            Previous
          </button>
          <div class="flex gap-1">
            <button v-for="page in visiblePages" :key="page" @click="changePage(page)" :class="[
              'px-3 py-1 border rounded-lg transition-colors',
              page === persons.page
                ? 'bg-sky-600 dark:bg-sky-500 text-white border-sky-600 dark:border-sky-500'
                : 'border-gray-300 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 text-gray-700 dark:text-gray-300'
            ]">
              {{ page }}
            </button>
          </div>
          <button @click="changePage(persons.page + 1)" :disabled="persons.page >= totalPages"
            class="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300">
            Next
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, onMounted, computed } from 'vue'
import { getPersons, deletePerson } from '@/services/Person/persons'
import ConfirmDialog from '@/components/Form/ConfirmAction.vue'

const persons = ref({
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
  sort: 'Name',
  dir: 'asc'
})

let debounceTimer = null

const emit = defineEmits(['edit', 'create'])

const totalPages = computed(() => {
  return Math.ceil(persons.value.totalItems / persons.value.pageSize)
})

const visiblePages = computed(() => {
  const current = persons.value.page
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
  await loadPersons()
})

const loadPersons = async () => {
  loading.value = true
  error.value = ''
  try {
    const params = {
      page: persons.value.page,
      pageSize: persons.value.pageSize,
      search: filters.value.search || undefined,
      sort: filters.value.sort,
      dir: filters.value.dir
    }
    const response = await getPersons(params)
    persons.value = response.data
  } catch (err) {
    error.value = 'Error al cargar clientes'
  } finally {
    loading.value = false
  }
}

const debouncedSearch = () => {
  clearTimeout(debounceTimer)
  debounceTimer = setTimeout(() => {
    persons.value.page = 1
    loadPersons()
  }, 500)
}

const toggleSort = (column) => {
  if (filters.value.sort === column) {
    filters.value.dir = filters.value.dir === 'asc' ? 'desc' : 'asc'
  } else {
    filters.value.sort = column
    filters.value.dir = 'asc'
  }
  loadPersons()
}

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    persons.value.page = page
    loadPersons()
  }
}

defineExpose({ loadPersons })

const handleDelete = async (id) => {
  const ok = await confirmDialog.value.ask(
    "Eliminar Cliente",
    "¿Estás seguro de que deseas eliminar este cliente?"
  )

  if (!ok) return

  try {
    await deletePerson(id)
    await loadPersons()
  } catch (err) {
    error.value = "Error deleting person"
  }
}


const handleEdit = (person) => {
  emit('edit', person)
}

const handleCreate = () => {
  emit('create')
}
</script>
