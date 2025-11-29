import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
})

const firstNames = [
    'Juan', 'María', 'Carlos', 'Ana', 'Luis', 'Carmen', 'José', 'Laura', 'Pedro', 'Isabel',
    'Miguel', 'Patricia', 'Antonio', 'Rosa', 'Francisco', 'Lucía', 'Manuel', 'Elena', 'Javier', 'Marta',
    'Sergio', 'Cristina', 'Rafael', 'Beatriz', 'Fernando', 'Silvia', 'Alberto', 'Raquel', 'Diego', 'Teresa',
    'Jorge', 'Pilar', 'Roberto', 'Natalia', 'Andrés', 'Sandra', 'Óscar', 'Mónica', 'Enrique', 'Andrea',
    'Pablo', 'Eva', 'Rubén', 'Julia', 'Gonzalo', 'Sofía', 'Daniel', 'Clara', 'Alejandro', 'Victoria'
]

const lastNames = [
    'García', 'Rodríguez', 'González', 'Fernández', 'López', 'Martínez', 'Sánchez', 'Pérez', 'Gómez', 'Martín',
    'Jiménez', 'Ruiz', 'Hernández', 'Díaz', 'Moreno', 'Álvarez', 'Muñoz', 'Romero', 'Alonso', 'Gutiérrez',
    'Navarro', 'Torres', 'Domínguez', 'Vázquez', 'Ramos', 'Gil', 'Ramírez', 'Serrano', 'Blanco', 'Suárez',
    'Molina', 'Castro', 'Ortega', 'Rubio', 'Marín', 'Sanz', 'Iglesias', 'Núñez', 'Medina', 'Garrido'
]

const secondLastNames = [
    'Silva', 'Mendoza', 'Cruz', 'Vargas', 'Ortiz', 'Reyes', 'Campos', 'Cortés', 'Flores', 'Rivera',
    'Aguilar', 'Herrera', 'Salazar', 'Méndez', 'Peña', 'Ríos', 'Santos', 'León', 'Morales', 'Valencia',
    'Chávez', 'Rojas', 'Fuentes', 'Contreras', 'Espinoza', 'Carrillo', 'Sandoval', 'Vega', 'Luna', 'Cabrera',
    'Guerrero', 'Castillo', 'Ponce', 'Soto', 'Delgado', 'Acosta', 'Benítez', 'Zamora', 'Pacheco', 'Paredes'
]

const emailDomains = ['gmail.com', 'hotmail.com', 'yahoo.com', 'outlook.com', 'empresa.com']

function generateRandomCI() {
    return Math.floor(1000000 + Math.random() * 9000000).toString()
}

function generateRandomPhone() {
    return Math.random() > 0.3 ? `+591 ${Math.floor(60000000 + Math.random() * 20000000)}` : null
}

function createPerson(index) {
    const firstName = firstNames[index % firstNames.length]
    const lastName = lastNames[Math.floor(Math.random() * lastNames.length)]
    const secondLastName = secondLastNames[Math.floor(Math.random() * secondLastNames.length)]
    const email = `${firstName.toLowerCase()}.${lastName.toLowerCase()}${index}@${emailDomains[Math.floor(Math.random() * emailDomains.length)]}`

    return {
        ci: generateRandomCI(),
        name: firstName,
        lastName: lastName,
        secondLastName: secondLastName,
        email: email,
        phone: generateRandomPhone()
    }
}

async function seedPersons() {
    console.log('🌱 Starting to seed 50 persons...')

    const persons = []
    for (let i = 0; i < 50; i++) {
        persons.push(createPerson(i))
    }

    let successCount = 0
    let errorCount = 0

    for (const person of persons) {
        try {
            await api.post('/persons', person)
            successCount++
            console.log(`✅ Created person ${successCount}/50: ${person.name} ${person.lastName}`)
        } catch (error) {
            errorCount++
            console.error(`❌ Error creating person: ${person.name} ${person.lastName}`, error.response?.data || error.message)
        }
    }

    console.log('\n📊 Summary:')
    console.log(`✅ Successfully created: ${successCount}`)
    console.log(`❌ Errors: ${errorCount}`)
    console.log('🎉 Seeding completed!')
}

seedPersons()
