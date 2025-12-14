#!/bin/bash

# Script d'arrêt du projet Le Barbu

echo "🛑 Arrêt du projet Le Barbu..."
echo ""

# Arrêter les processus Node.js (frontend)
echo "🎨 Arrêt du frontend..."
pkill -f "node.*nuxt" || echo "Frontend déjà arrêté"

# Arrêter les processus dotnet (backend)
echo "🔧 Arrêt du backend..."
pkill -f "dotnet.*Barbu.Api" || echo "Backend déjà arrêté"

# Arrêter PostgreSQL
echo "📦 Arrêt de PostgreSQL..."
cd infra
docker compose down
cd ..

echo ""
echo "✅ Tous les services ont été arrêtés"
