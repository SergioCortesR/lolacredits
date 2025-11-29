<template>
    <div v-if="open" class="fixed inset-0 backdrop-blur-xs flex items-center justify-center z-50">
        <div class="bg-white dark:bg-gray-800 p-6 rounded-xl shadow-xl dark:shadow-gray-900/50 border border-gray-200 dark:border-gray-700 w-full max-w-sm">
            <h2 class="text-lg font-semibold text-gray-800 dark:text-gray-100">{{ title }}</h2>
            <p class="text-gray-600 dark:text-gray-400 mt-2">{{ message }}</p>

            <div class="flex justify-end gap-3 mt-6">
                <button @click="cancel" class="px-4 py-2 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors">
                    Cancelar
                </button>
                <button @click="confirm" class="px-4 py-2 bg-sky-600 dark:bg-sky-500 text-white rounded-lg hover:bg-sky-700 dark:hover:bg-sky-600 transition-colors">
                    Confirmar
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref } from "vue"

const open = ref(false)
const title = ref('')
const message = ref('')
let _resolve

const ask = (t, m) => {
    title.value = t
    message.value = m
    open.value = true

    return new Promise((resolve) => {
        _resolve = resolve
    })
}

const confirm = () => {
    open.value = false
    _resolve(true)
}

const cancel = () => {
    open.value = false
    _resolve(false)
}

defineExpose({ ask })
</script>
