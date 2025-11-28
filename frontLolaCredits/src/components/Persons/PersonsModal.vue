<template>
  <div v-if="isOpen" class="fixed inset-0 backdrop-blur-xs p-4 rounded-xl flex items-center justify-center z-10">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <h2 class="text-lg font-semibold text-gray-900 mb-4">
        {{ editingPerson ? 'Edit Person' : 'New Person' }}
      </h2>

      <div v-if="error" class="mb-4 bg-red-50 border border-red-200 text-red-700 px-3 py-2 rounded text-sm">
        {{ error }}
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <Input v-model="form.ci" label="CI" placeholder="12345678" required />
        <Input v-model="form.name" label="Nombre" placeholder="Roberto" required />
        <Input v-model="form.lastName" label="Apellido" placeholder="Martinez" required />
        <Input v-model="form.secondLastName" label="Segundo apellido" placeholder="Delgado" required />
        <Input v-model="form.phone" label="Telefono" placeholder="+1234567890" />
        <Input v-model="form.email" label="Email" type="email" placeholder="roberto4@example.com" required />

        <div class="flex gap-3 justify-end pt-4">
          <button type="button" @click="close"
            class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">
            Cancel
          </button>
          <button type="submit" :disabled="isLoading"
            class="px-4 py-2 bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white rounded-lg transition-colors">
            {{ isLoading ? 'Saving...' : 'Save' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import Input from '@/components/Form/Input.vue'
import { getPerson } from '@/services/Person/persons'
import { createPerson, updatePerson } from '@/services/Person/persons'

const emit = defineEmits(['done'])

const isOpen = ref(false)
const isLoading = ref(false)
const error = ref('')
const editingPerson = ref(null)

const form = ref({
  name: '',
  lastName: '',
  secondLastName: '',
  ci: '',
  phone: '',
  email: ''
})

const open = (person = null) => {
  editingPerson.value = person
  if (person) {
    getPerson(person.id)
      .then(response => form.value = { ...response.data })
      .catch((err) => {
        console.error(err)
        error.value = "Error loading person"
      })
  } else {
    form.value = {
      name: "",
      lastName: "",
      secondLastName: "",
      ci: "",
      phone: "",
      email: ""
    }
  }
  error.value = ""
  isOpen.value = true
}

const close = () => {
  isOpen.value = false
  form.value = {
    name: '',
    lastName: '',
    secondLastName: '',
    ci: '',
    phone: '',
    email: ''
  }
}

const submit = async () => {
  isLoading.value = true
  error.value = ''
  console.log(form.value);

  try {
    if (editingPerson.value) {
      await updatePerson(editingPerson.value.id, form.value)
    } else {
      await createPerson(form.value)
    }
    emit('done')
    close()
  } catch (err) {
    error.value = 'Error saving person'
    console.error(err)
  } finally {
    isLoading.value = false
  }
}

defineExpose({ open })
</script>
