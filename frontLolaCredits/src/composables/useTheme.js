import { ref, watch } from 'vue'

// Estado global compartido
const isDark = ref(false)
const isInitialized = ref(false)

// Función para actualizar el tema en el DOM
const updateTheme = (value) => {
  if (value) {
    document.documentElement.classList.add('dark')
    localStorage.setItem('theme', 'dark')
  } else {
    document.documentElement.classList.remove('dark')
    localStorage.setItem('theme', 'light')
  }
}

// Watch global (solo se ejecuta una vez)
watch(isDark, (newValue) => {
  updateTheme(newValue)
}, { immediate: false })

export function useTheme() {
  const toggleTheme = () => {
    isDark.value = !isDark.value
  }

  const initTheme = () => {
    if (isInitialized.value) return
    
    const savedTheme = localStorage.getItem('theme')
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches
    
    isDark.value = savedTheme === 'dark' || (!savedTheme && prefersDark)
    updateTheme(isDark.value)
    isInitialized.value = true
  }

  return {
    isDark,
    toggleTheme,
    initTheme
  }
}

