<template>
  <div class="container mx-auto px-4 py-8">
    <div class="mb-8">
      <h1 class="text-4xl font-bold text-gray-900">Gestión de préstamos</h1>
      <p class="text-gray-600 mt-2">Crea, edita y gestiona préstamos</p>
    </div>

    <div class="bg-white rounded-lg shadow border border-gray-200 p-6">
      <LoansTable ref="loansTable" @create="handleCreate" @edit="handleEdit"
        @viewInstallments="handleViewInstallments" />
    </div>

    <LoansModal ref="modal" @done="handleDone" />
    <InstallmentsModal ref="installmentsModal" @paymentDone="handlePaymentDone" />
  </div>
</template>
<script setup>
import { ref } from 'vue'
import LoansTable from '@/components/Loans/LoansTable.vue'
import LoansModal from '@/components/Loans/LoansModal.vue'
import InstallmentsModal from '@/components/Loans/InstallmentsModal.vue'

const loansTable = ref(null);
const modal = ref(null)
const installmentsModal = ref(null)

const handleCreate = () => {
  modal.value?.open()
}

const handleEdit = (loan) => {
  modal.value?.open(loan)
}

const handleViewInstallments = (loan) => {
  installmentsModal.value?.open(loan)
}

const handleDone = async () => {
  await loansTable.value?.loadLoans()
}

const handlePaymentDone = async () => {
  await loansTable.value?.loadLoans()
}
</script>
