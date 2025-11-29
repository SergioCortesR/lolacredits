<template>
  <div class="container mx-auto px-4 py-8">
    <div class="mb-8">
      <h1 class="text-4xl font-bold text-gray-900">Gestión de prestamos</h1>
      <p class="text-gray-600 mt-2">Crea, edita y gestiona prestamos</p>
    </div>

    <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
      <LoansTable ref="loansTable" @create="handleCreate" @edit="handleEdit" />
    </div>

    <LoansModal ref="modal" @done="handleDone" />
  </div>
</template>
<script setup>
import { ref } from 'vue'
import LoansTable from '@/components/Loans/LoansTable.vue'
import LoansModal from '@/components/Loans/LoansModal.vue'

const loansTable = ref(null);
const modal = ref(null)

const handleCreate = () => {
  modal.value?.open()
}

const handleEdit = (loan) => {
  modal.value?.open(loan)
}

const handleDone = async () => {
  await loansTable.value?.loadLoans()
}
</script>
