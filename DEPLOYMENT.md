# LolaCredits - Guía de Despliegue

## 📋 Prerrequisitos
- Cuenta en GitHub
- Cuenta en Railway.app (gratis)
- Git instalado localmente

---

## 🚀 Opción 1: Railway.app (Recomendada)

### Paso 1: Preparar el repositorio

```bash
# Asegúrate de estar en la raíz del proyecto
cd c:/Users/cors0/OneDrive/Documentos/2_Proyectos/lolacredits

# Inicializar git si no está inicializado
git init
git add .
git commit -m "Initial commit - LolaCredits ready for deployment"

# Subir a GitHub
git remote add origin https://github.com/TU_USUARIO/lolacredits.git
git branch -M main
git push -u origin main
```

### Paso 2: Desplegar Backend en Railway

1. Ve a [Railway.app](https://railway.app) y crea una cuenta
2. Click en "New Project" → "Deploy from GitHub repo"
3. Selecciona el repo `lolacredits`
4. Railway detectará automáticamente el Dockerfile del backend
5. Configura las variables de entorno:
   - Click en el servicio → "Variables"
   - Agrega: `FRONTEND_URL` = (lo agregarás después de desplegar el frontend)
6. Click en "Deploy"
7. Una vez desplegado, copia la URL del backend (ej: `https://backlolacredits-production.up.railway.app`)

### Paso 3: Desplegar Frontend en Railway

1. En el mismo proyecto de Railway, click en "New Service"
2. Selecciona "GitHub Repo" → mismo repositorio
3. En Settings:
   - Root Directory: `frontLolaCredits`
   - Build Command: `npm run build`
4. Agrega variable de entorno:
   - `VITE_API_BASE_URL` = `https://TU-BACKEND-URL.railway.app/api`
5. Click en "Deploy"
6. Copia la URL del frontend

### Paso 4: Actualizar CORS

1. Ve al servicio del backend en Railway
2. Actualiza la variable `FRONTEND_URL` con la URL del frontend
3. El servicio se redesplega automáticamente

### Paso 5: Verificar

- Visita tu frontend: `https://tu-app.railway.app`
- Verifica que la API funcione: `https://tu-backend.railway.app/swagger`

---

## 🐳 Opción 2: Docker + VPS (DigitalOcean/Linode)

### Costo: ~$6/mes por un Droplet básico

```bash
# En tu VPS (después de instalar Docker y Docker Compose)

# 1. Clonar el repo
git clone https://github.com/TU_USUARIO/lolacredits.git
cd lolacredits

# 2. Crear docker-compose.yml (ver abajo)

# 3. Desplegar
docker-compose up -d

# 4. Verificar
docker-compose logs -f
```

**docker-compose.yml**:
```yaml
version: '3.8'

services:
  backend:
    build:
      context: ./backLolaCredits
      dockerfile: Dockerfile
    ports:
      - "5228:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - FRONTEND_URL=http://tu-dominio.com
    volumes:
      - ./data:/app/data
    restart: unless-stopped

  frontend:
    build:
      context: ./frontLolaCredits
      dockerfile: Dockerfile
    ports:
      - "80:80"
    environment:
      - VITE_API_BASE_URL=http://tu-dominio.com:5228/api
    depends_on:
      - backend
    restart: unless-stopped
```

---

## 🌐 Opción 3: Vercel (Frontend) + Railway (Backend)

### Frontend en Vercel (Gratis)

1. Ve a [Vercel.com](https://vercel.com)
2. Importa tu repo de GitHub
3. Configura:
   - Framework: Vite
   - Root Directory: `frontLolaCredits`
   - Build Command: `npm run build`
   - Output Directory: `dist`
4. Variables de entorno:
   - `VITE_API_BASE_URL` = URL de tu backend en Railway

### Backend en Railway (Gratis hasta 500 horas/mes)

Sigue los pasos del "Paso 2" de la Opción 1

---

## 📱 Opción 4: Netlify (Frontend) + Render (Backend)

Similar a Vercel + Railway, pero con Render.com para el backend.

---

## ⚙️ Configuración Post-Despliegue

### 1. Base de datos persistente
En Railway, el SQLite se guarda automáticamente. Para VPS:
```bash
# Crear volumen persistente para SQLite
docker volume create lolacredits-data
```

### 2. SSL/HTTPS (Importante)
- Railway y Vercel incluyen SSL automático
- Para VPS, usa Certbot con Let's Encrypt

### 3. Dominio personalizado
- Railway/Vercel: Agregar dominio custom en settings
- Configurar DNS: CNAME apuntando a la URL de Railway/Vercel

---

## 🔒 Checklist de Seguridad antes de Producción

- [ ] Cambiar CORS para permitir solo tu dominio
- [ ] Agregar rate limiting en el backend
- [ ] Implementar autenticación (JWT)
- [ ] Usar variables de entorno para secretos
- [ ] Configurar HTTPS/SSL
- [ ] Agregar logging (Serilog)
- [ ] Backup automático de la base de datos

---

## 📊 Monitoreo

### Railway
- Dashboard integrado con logs y métricas
- Alertas automáticas

### Sentry (Recomendado para errores)
```bash
npm install @sentry/vue
```

---

## 🎯 Recomendación Final

**Para empezar rápido**: Railway (backend + frontend)  
**Para producción seria**: Railway (backend) + Vercel (frontend)  
**Para máximo control**: VPS con Docker Compose  
**Para aprender DevOps**: GitHub Actions + DigitalOcean

La opción más rápida es Railway - 10 minutos y está todo deployado.
