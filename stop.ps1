# Script d'arrêt du projet Le Barbu

Write-Host "🛑 Arrêt du projet Le Barbu..." -ForegroundColor Cyan
Write-Host ""

# Arrêter les processus Node.js (frontend)
Write-Host "🎨 Arrêt du frontend..." -ForegroundColor Yellow
Get-Process -Name "node" -ErrorAction SilentlyContinue | Where-Object {$_.Path -like "*barbu-nuxt*"} | Stop-Process -Force
if ($?) {
    Write-Host "✅ Frontend arrêté" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Frontend déjà arrêté" -ForegroundColor Gray
}

# Arrêter les processus dotnet (backend)
Write-Host "🔧 Arrêt du backend..." -ForegroundColor Yellow
Get-Process -Name "Barbu.Api" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object {$_.Path -like "*Barbu*"} | Stop-Process -Force
if ($?) {
    Write-Host "✅ Backend arrêté" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Backend déjà arrêté" -ForegroundColor Gray
}

# Arrêter PostgreSQL
Write-Host "📦 Arrêt de PostgreSQL..." -ForegroundColor Yellow
Set-Location infra
docker compose down | Out-Null
Set-Location ..
Write-Host "✅ PostgreSQL arrêté" -ForegroundColor Green

Write-Host ""
Write-Host "✅ Tous les services ont été arrêtés" -ForegroundColor Green
