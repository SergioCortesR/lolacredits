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
  modelValue: [String, Number]
})

defineEmits(['update:modelValue'])
</script>

<template>
  <div class="space-y-1">
    <label v-if="label" class="block text-sm font-medium text-gray-700">
      {{ label }}
      <span v-if="required" class="text-red-600">*</span>
    </label>
    <input
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      :type="type"
      :placeholder="placeholder"
      :required="required"
      class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
    />
    <p v-if="error" class="text-sm text-red-600">{{ error }}</p>
  </div>
</template>