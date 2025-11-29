<script setup>
defineProps({
  label: String,
  placeholder: String,
  type: {
    type: String,
    default: 'text'
  },
  required: Boolean,
  error: String,
  modelValue: [String, Number],
  disabled: {
    type: Boolean,
    default: false
  },
  min: [String, Number],
  max: [String, Number],
  step: [String, Number]
})

defineEmits(['update:modelValue'])
</script>

<template>
  <div class="space-y-1">
    <label v-if="label" class="block text-sm font-medium text-gray-700 dark:text-gray-300">
      {{ label }}
      <span v-if="required" class="text-red-600 dark:text-red-400">*</span>
    </label>
    <input
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      :type="type"
      :placeholder="placeholder"
      :required="required"
      :disabled="disabled"
      :min="min"
      :max="max"
      :step="step"
      :class="[
        'w-full px-3 py-2 border rounded-lg focus:outline-none text-gray-900 dark:text-gray-100',
        disabled 
          ? 'bg-gray-100 dark:bg-gray-700 border-gray-200 dark:border-gray-600 cursor-not-allowed text-gray-500 dark:text-gray-400' 
          : 'bg-white dark:bg-gray-700 border-gray-300 dark:border-gray-600 focus:ring-2 focus:ring-sky-500 dark:focus:ring-sky-400 focus:border-transparent'
      ]"
    />
    <p v-if="error" class="text-sm text-red-600 dark:text-red-400">{{ error }}</p>
  </div>
</template>