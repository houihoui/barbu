#!/bin/bash

# Script de démarrage complet du projet Le Barbu

echo "🎮 Démarrage du projet Le Barbu..."
echo ""

# 1. Démarrer PostgreSQL
echo "📦 Démarrage de PostgreSQL..."
cd infra
docker compose up -d
if [ $? -ne 0 ]; then
    echo "❌ Erreur: Docker Desktop n'est pas démarré"
    echo "➡️  Veuillez lancer Docker Desktop puis relancer ce script"
    exit 1
fi
cd ..
echo "✅ PostgreSQL démarré"
echo ""

# 2. Démarrer le backend
echo "🔧 Démarrage du backend .NET..."
cd backend/src/Barbu.Api
dotnet run &
BACKEND_PID=$!
cd ../../..
echo "✅ Backend démarré (PID: $BACKEND_PID)"
echo "   URL: http://localhost:5000"
echo ""

# Attendre que le backend soit prêt
echo "⏳ Attente du démarrage du backend..."
sleep 5
echo ""

# 3. Démarrer le frontend
echo "🎨 Démarrage du frontend Nuxt..."
cd frontend/barbu-nuxt
npm run dev &
FRONTEND_PID=$!
cd ../..
echo "✅ Frontend démarré (PID: $FRONTEND_PID)"
echo "   URL: http://localhost:3000"
echo ""

echo "✨ Projet démarré avec succès !"
echo ""
echo "📋 Services en cours d'exécution:"
echo "   - PostgreSQL: localhost:5432"
echo "   - Backend API: http://localhost:5000"
echo "   - Frontend: http://localhost:3000"
echo ""
echo "Pour arrêter les services, utilisez ./stop.sh"
echo ""

# Garder le script actif
wait
