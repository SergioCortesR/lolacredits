# Railway Deployment - Quick Start Guide

## 🚀 Deploy LolaCredits to Railway in 5 Minutes

### Prerequisites
- GitHub account
- Railway account (free at [railway.app](https://railway.app))
- Git repository pushed to GitHub

---

## Step 1: Push to GitHub

```bash
cd c:/Users/cors0/OneDrive/Documentos/2_Proyectos/lolacredits

# Initialize git if not done
git init
git add .
git commit -m "Ready for Railway deployment"

# Create repo on GitHub, then:
git remote add origin https://github.com/YOUR_USERNAME/lolacredits.git
git branch -M main
git push -u origin main
```

---

## Step 2: Deploy Backend to Railway

1. **Go to [Railway.app](https://railway.app)** and login with GitHub

2. **Create New Project**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose your `lolacredits` repository
   - Railway will auto-detect it's a .NET project

3. **Configure Backend Service**
   - Click on the deployed service
   - Go to "Settings" → "Service Name" → Rename to `backend`
   - Go to "Settings" → "Root Directory" → Set to `backLolaCredits`

4. **Add Environment Variables**
   - Go to "Variables" tab
   - Add these variables:
     ```
     ASPNETCORE_ENVIRONMENT = Production
     FRONTEND_URL = (leave empty for now, will update after frontend deploy)
     ```

5. **Add Volume for Database** (Important!)
   - Go to "Settings" → "Volumes"
   - Click "Add Volume"
   - Mount Path: `/data`
   - This ensures your SQLite database persists across deployments

6. **Get Backend URL**
   - Go to "Settings" → "Networking" → "Generate Domain"
   - Copy the URL (e.g., `https://backlolacredits-production.up.railway.app`)
   - Save it for the next step

7. **Wait for Deployment**
   - Railway will build and deploy automatically
   - Check "Deployments" tab for status
   - Once deployed, test: `https://YOUR-BACKEND-URL/swagger`

---

## Step 3: Deploy Frontend to Railway

1. **Add New Service to Same Project**
   - In your Railway project, click "New Service"
   - Select "GitHub Repo" → Same `lolacredits` repo

2. **Configure Frontend Service**
   - Click on the service → "Settings"
   - Service Name: `frontend`
   - Root Directory: `frontLolaCredits`

3. **Add Environment Variables**
   - Go to "Variables" tab
   - Add:
     ```
     VITE_API_BASE_URL = https://YOUR-BACKEND-URL.up.railway.app/api
     ```
   - Replace `YOUR-BACKEND-URL` with the URL from Step 2.6

4. **Generate Domain**
   - Go to "Settings" → "Networking" → "Generate Domain"
   - Copy frontend URL (e.g., `https://lolacredits.up.railway.app`)

5. **Wait for Deployment**
   - Check "Deployments" tab
   - Once deployed, visit your frontend URL

---

## Step 4: Update Backend CORS

1. **Go back to Backend Service**
   - Click on backend service
   - Go to "Variables" tab

2. **Update FRONTEND_URL**
   - Set `FRONTEND_URL` = `https://YOUR-FRONTEND-URL.up.railway.app`
   - Railway will automatically redeploy

---

## Step 5: Test Your App! 🎉

Visit your frontend URL and verify:
- ✅ Dashboard loads
- ✅ Can create persons
- ✅ Can create loans
- ✅ Dark mode works
- ✅ All features functional

**Swagger API Docs:** `https://YOUR-BACKEND-URL.up.railway.app/swagger`

---

## 🔧 Troubleshooting

### Backend won't start
- Check "Deployments" → "Logs" for errors
- Verify volume is mounted at `/data`
- Ensure `railway.toml` exists in `backLolaCredits/`

### Frontend shows API errors
- Verify `VITE_API_BASE_URL` in frontend variables
- Check CORS: Ensure backend has correct `FRONTEND_URL`
- Test backend directly: `https://YOUR-BACKEND-URL/api/dashboard/stats`

### Database resets on redeploy
- Ensure Volume is properly configured
- Mount path must be `/data`
- ConnectionString should use `/data/LolaCredits.db`

### Build fails
- Check logs in "Deployments" tab
- Verify `nixpacks.toml` files exist
- Ensure Node 20+ and .NET 8 are specified

---

## 🎯 Custom Domain (Optional)

1. Go to Service → "Settings" → "Networking"
2. Click "Custom Domain"
3. Enter your domain (e.g., `lolacredits.com`)
4. Add CNAME record in your DNS:
   - Name: `@` or `www`
   - Value: Provided by Railway
5. Wait for DNS propagation (~5-30 minutes)

---

## 💰 Railway Pricing

**Hobby Plan (Free):**
- $5 free credit per month
- ~500 execution hours
- Perfect for personal projects

**Pro Plan ($20/month):**
- $20 credit + usage-based
- Unlimited projects
- For production apps

---

## 🔄 Auto-Deploy on Git Push

Railway automatically redeploys when you push to `main` branch:

```bash
git add .
git commit -m "Update feature"
git push origin main
```

Railway detects changes and redeploys automatically! 🚀

---

## 📊 Monitoring

- **Logs:** Click service → "Logs" tab
- **Metrics:** View CPU, Memory, Network usage
- **Deployments:** Track deployment history and rollback if needed

---

## ⚡ Quick Commands

```bash
# View backend logs
railway logs --service backend

# View frontend logs  
railway logs --service frontend

# Open backend in browser
railway open --service backend

# Redeploy manually
railway up --service backend
```

---

## 🎓 Summary Checklist

- [ ] Code pushed to GitHub
- [ ] Backend deployed to Railway with volume
- [ ] Frontend deployed to Railway
- [ ] Environment variables configured
- [ ] CORS updated with frontend URL
- [ ] App tested and working
- [ ] Custom domain added (optional)

---

**Need help?** Check Railway docs or the main [DEPLOYMENT.md](./DEPLOYMENT.md) for other deployment options.
