<template>
    <div v-if="open" class="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
        <div class="bg-white p-6 rounded-xl shadow-xl w-full max-w-sm">
            <h2 class="text-lg font-semibold text-gray-800">{{ title }}</h2>
            <p class="text-gray-600 mt-2">{{ message }}</p>

            <div class="flex justify-end gap-3 mt-6">
                <button @click="cancel" class="px-4 py-2 bg-gray-200 rounded-lg hover:bg-gray-300">
                    Cancel
                </button>
                <button @click="confirm" class="px-4 py-2 bg-blue-700 text-white rounded-lg hover:bg-blue-500">
                    Confirm
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
