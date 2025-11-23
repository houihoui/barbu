# Le Barbu - Application de Gestion

Application web complète pour gérer le jeu de cartes **Le Barbu**.

## Architecture

- **Backend** : .NET 10 (C#), ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- **Frontend** : Nuxt 3 (TypeScript), Pinia, Tailwind CSS
- **Base de données** : PostgreSQL (hébergée sur Neon)

## Structure du projet

```
Barbu/
├── backend/
│   ├── Barbu.sln
│   └── src/
│       ├── Barbu.Api/          # API REST
│       ├── Barbu.Domain/        # Entités métier
│       └── Barbu.Infrastructure/ # DbContext, EF Core
├── frontend/
│   └── barbu-nuxt/              # Application Nuxt 3
├── infra/
│   └── docker-compose.yml       # (à venir)
└── README.md
```

## Prérequis

- .NET 10 SDK
- Node.js 18+ et npm
- PostgreSQL (local ou Neon pour dev)
- Git

## Installation

### Backend

```bash
cd backend

# Restaurer les dépendances
dotnet restore

# Configurer la base de données
# Modifier appsettings.Development.json avec vos infos Neon
# ou utiliser PostgreSQL local (appsettings.json)

# Compiler
dotnet build

# Lancer l'API (depuis backend/src/Barbu.Api)
cd src/Barbu.Api
dotnet run
```

L'API sera disponible sur `https://localhost:5001` (ou le port configuré).

### Frontend

```bash
cd frontend/barbu-nuxt

# Installer les dépendances
npm install

# Lancer en mode développement
npm run dev
```

L'application sera disponible sur `http://localhost:3000`.

## Configuration de la base de données

### Option 1 : PostgreSQL local

Utilisez la connection string dans `backend/src/Barbu.Api/appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=barbu_dev;Username=postgres;Password=postgres"
}
```

### Option 2 : Neon (cloud)

Créez une base de données sur [Neon](https://neon.tech) et mettez à jour `appsettings.Development.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=YOUR_NEON_HOST;Database=YOUR_DATABASE;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SSL Mode=Require"
}
```

### Migrations EF Core

```bash
# Depuis backend/src/Barbu.Api
dotnet ef migrations add InitialCreate --project ../Barbu.Infrastructure
dotnet ef database update
```

## Fonctionnalités

### Vue d'ensemble

L'application permet de gérer :

- **Joueurs** : CRUD avec nom, avatar, statistiques
- **Parties** : Création à 3 ou 4 joueurs, 7 contrats par joueur
- **Contrats** : Barbu, Pas de plis, Cœurs, Dames, 2 derniers plis, Atout, Réussite
- **Contres/Surcontres** : Système de duels entre joueurs
- **LIVE** : Vue en lecture seule d'une partie en cours
- **Historique** : Liste des parties (en cours/terminées)
- **Championnat** : Gestion de groupes de joueurs avec barème 3/4 joueurs

### Navigation

- **Joueurs** : Gestion des joueurs
- **Partie** : Création et gestion d'une partie
- **LIVE** : Visualisation temps réel
- **Historique** : Liste des parties
- **Championnat** : Classement et points

## Règles de scoring (synthèse)

### Contrats

- **Barbu** : Éviter le Roi de Cœur (-20)
- **Pas de plis** : Chaque pli = -2
- **Cœurs** : Chaque cœur = -2, As = -6
- **Dames** : Chaque Dame = -6
- **2 derniers plis** : Avant-dernier = -10, dernier = -20
- **Atout** : Chaque pli = +5 (contrat positif)
- **Réussite** : 1er = +45, 2e = +20, 3e = +10, 4e = -10

### Contres/Surcontres

- Contre : transfert de l'écart entre 2 joueurs
- Surcontre : transfert de 2× l'écart
- Pour Atout et Réussite : seuls les flancs peuvent contrer le déclarant

### Championnat

**4 joueurs** :
- 1er : 10 + (score/10 arrondi à 0,5)
- 2e : 5 + (score/10 arrondi à 0,5)
- 3e : 2 + (score/10 arrondi à 0,5)
- 4e : 0 + (score/10 arrondi à 0,5)

**3 joueurs** :
- 1er : 8 + (score/10 arrondi à 0,5)
- 2e : 3 + (score/10 arrondi à 0,5)
- 3e : 0 + (score/10 arrondi à 0,5)

## Développement

### Technologies

**Backend** :
- ASP.NET Core 10
- Entity Framework Core 10
- Npgsql (driver PostgreSQL)
- ASP.NET Identity + JWT (auth à venir)

**Frontend** :
- Nuxt 3.4+
- Vue 3 avec Composition API
- TypeScript (strict mode)
- Pinia (state management)
- Tailwind CSS

### Scripts utiles

**Backend** :
```bash
# Build
dotnet build

# Tests (à venir)
dotnet test

# Migrations
dotnet ef migrations add <MigrationName> --project src/Barbu.Infrastructure
dotnet ef database update
```

**Frontend** :
```bash
# Dev
npm run dev

# Build
npm run build

# Preview
npm run preview
```

## À faire

- [ ] Implémenter les entités Domain (Player, Game, Contract, etc.)
- [ ] Créer les repositories et services
- [ ] Développer les endpoints API
- [ ] Implémenter l'authentification (ASP.NET Identity + JWT)
- [ ] Développer les composants frontend
- [ ] Implémenter la logique de calcul des scores
- [ ] Ajouter la gestion temps réel (SignalR ?)
- [ ] Tests unitaires et d'intégration
- [ ] Configuration Docker
- [ ] CI/CD

## Licence

Projet personnel - Usage libre
