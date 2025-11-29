<template>
  <div class="space-y-4">
    <!-- Error message -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <!-- Header with filters and Create button -->
    <div class="flex flex-col md:flex-row gap-4 justify-between items-start md:items-center">
      <div class="flex-1 w-full md:w-auto">
        <input v-model="filters.search" @input="debouncedSearch" type="text" placeholder="Buscar por nombre, CI, email..."
          class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" />
      </div>
      <button @click="handleCreate"
        class="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition-colors whitespace-nowrap">
        + Nuevo Cliente
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="text-center text-gray-500 py-8">Cargando...</div>

    <!-- Empty state -->
    <div v-else-if="!persons.items || persons.items.length === 0"
      class="bg-white p-6 rounded-lg border border-gray-200 text-center text-gray-500">
      No hay clientes aún. Crea uno para comenzar.
    </div>

    <!-- Table -->
    <div v-else class="space-y-4">
      <div class="overflow-x-auto">
        <table class="w-full border-collapse">
          <ConfirmDialog ref="confirmDialog" />
          <thead>
            <tr class="bg-gray-100 border-b border-gray-200">
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">
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
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">
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
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Phone</th>
              <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">
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
              <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="person in persons.items" :key="person.id" class="border-b border-gray-200 hover:bg-gray-50">
              <td class="px-4 py-3 text-sm text-gray-600">{{ person.ci }}</td>
              <td class="px-4 py-3 text-sm text-gray-700">{{ person.fullName }}</td>
              <td class="px-4 py-3 text-sm text-gray-600">{{ person.phone ?? 'N/A' }}</td>
              <td class="px-4 py-3 text-sm text-gray-700">{{ person.email }}</td>
              <td class="px-4 py-3 text-center space-x-2">
                <button @click="handleEdit(person)" class="text-blue-600 hover:text-blue-900 font-medium text-sm">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
                  </svg>
                </button>
                <button @click="handleDelete(person.id)" class="text-red-600 hover:text-red-900 font-medium text-sm">
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
          Mostrando del {{ (persons.page - 1) * persons.pageSize + 1 }} al {{ Math.min(persons.page * persons.pageSize,
            persons.totalItems) }} de {{ persons.totalItems }} resultados
        </div>
        <div class="flex items-center gap-2">
          <button @click="changePage(persons.page - 1)" :disabled="persons.page === 1"
            class="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed">
            Previous
          </button>
          <div class="flex gap-1">
            <button v-for="page in visiblePages" :key="page" @click="changePage(page)" :class="[
              'px-3 py-1 border rounded-lg',
              page === persons.page
                ? 'bg-blue-600 text-white border-blue-600'
                : 'border-gray-300 hover:bg-gray-50'
            ]">
              {{ page }}
            </button>
          </div>
          <button @click="changePage(persons.page + 1)" :disabled="persons.page >= totalPages"
            class="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed">
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
    console.error(err)
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
