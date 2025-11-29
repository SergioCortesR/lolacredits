/**
 * Formatea un número con separadores de miles
 * @param {number} value - El número a formatear
 * @param {number} decimals - Número de decimales (default: 2)
 * @returns {string} - Número formateado con comas
 */
export function formatCurrency(value, decimals = 2) {
  if (value === null || value === undefined || isNaN(value)) {
    return '0.00'
  }
  
  return Number(value).toLocaleString('en-US', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  })
}

/**
 * Formatea un número entero con separadores de miles
 * @param {number} value - El número a formatear
 * @returns {string} - Número formateado con comas
 */
export function formatNumber(value) {
  if (value === null || value === undefined || isNaN(value)) {
    return '0'
  }
  
  return Number(value).toLocaleString('en-US')
}
