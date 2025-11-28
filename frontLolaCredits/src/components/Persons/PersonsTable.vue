<template>
  <div class="space-y-4">
    <!-- Error message -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
      {{ error }}
    </div>

    <!-- Header with Create button -->
    <div class="flex justify-between items-center">
      <h2 class="text-lg font-semibold text-gray-900">Persons</h2>
      <button @click="handleCreate"
        class="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition-colors">
        + Nuevo Cliente
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="text-center text-gray-500">Loading...</div>

    <!-- Empty state -->
    <div v-else-if="persons.length === 0"
      class="bg-white p-6 rounded-lg border border-gray-200 text-center text-gray-500">
      No hay clientes aún. Crea uno para comenzar.
    </div>

    <!-- Table -->
    <div v-else class="overflow-x-auto">
      <table class="w-full border-collapse">
        <ConfirmDialog ref="confirmDialog" />
        <thead>
          <tr class="bg-gray-100 border-b border-gray-200">
            <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">CI</th>
            <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Name</th>
            <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Phone</th>
            <th class="px-4 py-3 text-left text-sm font-semibold text-gray-900">Email</th>
            <th class="px-4 py-3 text-center text-sm font-semibold text-gray-900">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="person in persons" :key="person.id" class="border-b border-gray-200 hover:bg-gray-50">
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
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { getPersons, deletePerson } from '@/services/Person/persons'
import ConfirmDialog from '@/components/Form/ConfirmAction.vue'

const persons = ref([]);
const loading = ref(false);
const confirmDialog = ref(null);
const error = ref('');

const emit = defineEmits(['edit', 'create'])

onMounted(async () => {
  await loadPersons()
})

const loadPersons = async () => {
  loading.value = true
  error.value = ''
  try {
    persons.value = (await getPersons()).data
    console.log(persons.value)
  } catch (err) {
    error.value = 'Error loading persons'
    console.error(err)
  } finally {
    loading.value = false
  }
}
defineExpose({ loadPersons })

const handleDelete = async (id) => {
  const ok = await confirmDialog.value.ask(
    "Delete Person",
    "Are you sure you want to delete this person?"
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
