# Railway Deployment Fix - Updated Instructions

## 🔧 Error Fix: "Build Image Error creating build plan with Railpack"

El error se solucionó actualizando las configuraciones. Ahora sigue estos pasos:

---

## 📋 Nueva Estrategia de Despliegue

Railway necesita que cada servicio esté en un proyecto separado o usar "Service from Template".

### ✅ Opción Recomendada: 2 Proyectos Separados

#### 1️⃣ Deploy Backend

1. **Ir a Railway**: https://railway.app/new
2. **Deploy from GitHub Repo** → Selecciona `lolacredits`
3. **Settings**:
   - Root Directory: `backLolaCredits`
   - Name: `lolacredits-backend`
4. **Add Volume** (IMPORTANTE):
   - Click "Add Volume"
   - Mount Path: `/data`
5. **Variables**:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   FRONTEND_URL=https://your-frontend-url.railway.app
   ```
6. **Generate Domain** en Settings → Networking
7. Copia la URL: `https://lolacredits-backend-xxxxx.up.railway.app`

#### 2️⃣ Deploy Frontend

1. **Nuevo Proyecto** en Railway: https://railway.app/new
2. **Deploy from GitHub Repo** → Mismo repo `lolacredits`
3. **Settings**:
   - Root Directory: `frontLolaCredits`
   - Name: `lolacredits-frontend`
4. **Variables**:
   ```
   VITE_API_BASE_URL=https://tu-backend-url.railway.app/api
   ```
   (Usa la URL del paso 1.7)
5. **Generate Domain**
6. Copia la URL del frontend

#### 3️⃣ Actualizar Backend CORS

1. Vuelve al proyecto del backend
2. Actualiza la variable `FRONTEND_URL` con la URL del frontend del paso 2.6
3. Railway redesplega automáticamente

---

## 🎯 Alternativa: Usar Railway CLI

Si prefieres línea de comandos:

```bash
# Instalar Railway CLI
npm i -g @railway/cli

# Login
railway login

# Deploy Backend
cd backLolaCredits
railway init
railway up

# Deploy Frontend (en otra terminal)
cd frontLolaCredits
railway init
railway up
```

---

## ✅ Verificación

Después de desplegar:

1. **Backend Swagger**: `https://tu-backend.railway.app/swagger`
2. **Frontend**: `https://tu-frontend.railway.app`
3. **Test API**: Crear una persona desde el frontend

---

## 🔄 Si todavía tienes errores

### Opción Simple: Usar Dockerfile

Railway puede usar Docker en lugar de Nixpacks:

1. En Railway Settings del servicio
2. Busca "Builder" 
3. Cambia de "Nixpacks" a "Dockerfile"
4. Railway usará el Dockerfile que ya tienes configurado

---

## 💡 Configuración Actual

Ya tienes estos archivos optimizados:
- ✅ `backLolaCredits/nixpacks.toml`
- ✅ `backLolaCredits/railway.toml`
- ✅ `backLolaCredits/Dockerfile`
- ✅ `frontLolaCredits/nixpacks.toml`
- ✅ `frontLolaCredits/railway.toml`
- ✅ `frontLolaCredits/Dockerfile`

Railway detectará automáticamente cuál usar.

---

## 🚨 Importante

**Root Directory** es clave:
- Backend: `backLolaCredits`
- Frontend: `frontLolaCredits`

Sin esto, Railway intenta construir desde la raíz y falla.
