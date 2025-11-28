<template>
  <div class="container mx-auto px-4 py-8">
    <div class="mb-8">
      <h1 class="text-4xl font-bold text-gray-900">Gestión de Clientes</h1>
      <p class="text-gray-600 mt-2">Crea, edita y gestiona clientes</p>
    </div>

    <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
      <PersonsTable ref="personsTable" @create="handleCreate" @edit="handleEdit" />
    </div>

    <PersonsModal ref="modal" @done="handleDone" />
  </div>
</template>
<script setup>
import { ref } from 'vue'
import PersonsTable from '@/components/Persons/PersonsTable.vue'
import PersonsModal from '@/components/Persons/PersonsModal.vue'

const personsTable = ref(null);
const modal = ref(null)

const handleCreate = () => {
  modal.value?.open()
}

const handleEdit = (person) => {
  modal.value?.open(person)
}

const handleDone = async () => {
  await personsTable.value?.loadPersons()
}
</script>
