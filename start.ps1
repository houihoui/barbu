# Script de démarrage complet du projet Le Barbu

Write-Host "🎮 Démarrage du projet Le Barbu..." -ForegroundColor Cyan
Write-Host ""

# 1. Démarrer PostgreSQL
Write-Host "📦 Démarrage de PostgreSQL..." -ForegroundColor Yellow
Set-Location infra
$dockerResult = docker compose up -d 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erreur: Docker Desktop n'est pas démarré" -ForegroundColor Red
    Write-Host "➡️  Veuillez lancer Docker Desktop puis relancer ce script" -ForegroundColor Yellow
    Set-Location ..
    exit 1
}
Set-Location ..
Write-Host "✅ PostgreSQL démarré" -ForegroundColor Green
Write-Host ""

# 2. Vérifier si le backend tourne déjà
$backendRunning = netstat -ano | Select-String ":5000" | Select-String "LISTENING"
if ($backendRunning) {
    Write-Host "ℹ️  Backend déjà en cours d'exécution sur http://localhost:5000" -ForegroundColor Cyan
} else {
    Write-Host "🔧 Démarrage du backend .NET..." -ForegroundColor Yellow
    Set-Location backend\src\Barbu.Api
    Start-Process -NoNewWindow powershell -ArgumentList "-Command", "dotnet run"
    Set-Location ..\..\..
    Write-Host "✅ Backend démarré" -ForegroundColor Green
    Write-Host "   URL: http://localhost:5000" -ForegroundColor Gray
    Write-Host ""

    # Attendre que le backend soit prêt
    Write-Host "⏳ Attente du démarrage du backend..." -ForegroundColor Yellow
    Start-Sleep -Seconds 5
    Write-Host ""
}

# 3. Démarrer le frontend
Write-Host "🎨 Démarrage du frontend Nuxt..." -ForegroundColor Yellow
Set-Location frontend\barbu-nuxt
Start-Process -NoNewWindow powershell -ArgumentList "-Command", "npm run dev"
Set-Location ..\..
Write-Host "✅ Frontend démarré" -ForegroundColor Green
Write-Host "   URL: http://localhost:3000 (ou port alternatif si occupé)" -ForegroundColor Gray
Write-Host ""

Write-Host "✨ Projet démarré avec succès !" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Services en cours d'exécution:" -ForegroundColor Cyan
Write-Host "   - PostgreSQL: localhost:5432" -ForegroundColor Gray
Write-Host "   - Backend API: http://localhost:5000" -ForegroundColor Gray
Write-Host "   - Frontend: http://localhost:3000 (vérifier la console)" -ForegroundColor Gray
Write-Host ""
Write-Host "Pour arrêter les services, utilisez .\stop.ps1" -ForegroundColor Yellow
Write-Host ""
