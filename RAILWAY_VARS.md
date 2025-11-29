# Railway Environment Variables for LolaCredits

## Backend Service Variables

# ASP.NET Core Environment
ASPNETCORE_ENVIRONMENT=Production

# Database path (Railway provides persistent storage)
ConnectionStrings__DefaultConnection=Data Source=/data/LolaCredits.db

# Frontend URL (update after deploying frontend)
FRONTEND_URL=${{RAILWAY_STATIC_URL}}

# Port (Railway provides this automatically)
PORT=8080

---

## Frontend Service Variables

# Backend API URL (update after deploying backend)
VITE_API_BASE_URL=https://${{BACKEND_RAILWAY_DOMAIN}}/api

# Port (Railway provides this automatically)
PORT=3000
