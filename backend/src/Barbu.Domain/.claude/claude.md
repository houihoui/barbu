# Barbu.Domain - Documentation des Entités

## Vue d'ensemble

Le projet **Barbu.Domain** contient toutes les entités métier et la logique du domaine pour le jeu de cartes "Le Barbu". Ce document décrit l'architecture du domaine, les entités et leurs relations.

## Architecture du domaine

Le domaine suit les principes de **Domain-Driven Design (DDD)** avec :
- Entités riches avec logique métier
- Relations explicites entre entités
- Enums pour les valeurs métier
- Validation des invariants du domaine

## Structure des dossiers

```
Barbu.Domain/
├── Entities/           # Entités du domaine
│   ├── Player.cs
│   ├── Game.cs
│   ├── GamePlayer.cs
│   ├── Deal.cs
│   ├── DealScore.cs
│   ├── Challenge.cs
│   ├── Championship.cs
│   └── ChampionshipPlayer.cs
└── Enums/             # Énumérations métier
    ├── ContractType.cs
    ├── GameStatus.cs
    └── ChallengeType.cs
```

## Enums

### ContractType

Les 7 types de contrats disponibles dans le jeu du Barbu :

| Valeur | Nom | Description | Points |
|--------|-----|-------------|--------|
| 1 | Barbu | Éviter de prendre le Roi de Cœur | -20 pour celui qui le prend |
| 2 | NoPlis | Éviter de prendre des plis | -2 par pli |
| 3 | Coeurs | Éviter de prendre des cœurs | -2 par cœur, -6 pour l'As |
| 4 | Dames | Éviter de prendre des Dames | -6 par Dame |
| 5 | DeuxDerniersPlis | Éviter les 2 derniers plis | -10 avant-dernier, -20 dernier |
| 6 | Atout | Prendre le plus de plis possible | +5 par pli (positif) |
| 7 | Reussite | Finir en 1ère position | 1er: +45, 2e: +20, 3e: +10, 4e: -10 |

**Règles spéciales** :
- Pour **Atout** et **Réussite** : seuls les flancs peuvent contrer le déclarant
- Pour les autres contrats : tous les joueurs peuvent se contrer mutuellement

### GameStatus

Statuts possibles d'une partie :

| Valeur | Nom | Description |
|--------|-----|-------------|
| 0 | Pending | Partie créée mais non démarrée |
| 1 | InProgress | Partie en cours |
| 2 | Completed | Partie terminée normalement |
| 3 | Abandoned | Partie abandonnée |

### ChallengeType

Types de défis entre joueurs :

| Valeur | Nom | Description | Multiplicateur |
|--------|-----|-------------|----------------|
| 1 | Contre | Défi standard entre 2 joueurs | ×1 (transfert de l'écart) |
| 2 | Surcontre | Contre-attaque du déclarant | ×2 (transfert de 2× l'écart) |

## Entités principales

### Player

Représente un joueur avec ses statistiques globales.

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `Name` (string, required, max 100) : Nom du joueur
- `Avatar` (string?, max 500) : URL ou chemin de l'avatar
- `GamesPlayed` (int) : Nombre total de parties jouées
- `Wins` (int) : Nombre de victoires
- `CreatedAt` (DateTime) : Date de création
- `UpdatedAt` (DateTime) : Date de dernière modification

**Relations** :
- `GamePlayers` : Liste des participations aux parties (collection de `GamePlayer`)
- `ChampionshipPlayers` : Liste des participations aux championnats (collection de `ChampionshipPlayer`)

**Index** :
- Index sur `Name` pour recherche rapide

---

### Game

Représente une partie de Barbu (3 ou 4 joueurs).

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `Name` (string?, max 200) : Nom optionnel de la partie
- `PlayerCount` (int) : Nombre de joueurs (3 ou 4)
- `Status` (GameStatus) : Statut actuel de la partie
- `CurrentDealNumber` (int) : Numéro de la donne en cours (1 à 21 ou 28)
- `StartedAt` (DateTime) : Date de début
- `CompletedAt` (DateTime?) : Date de fin (null si en cours)
- `CreatedAt` (DateTime) : Date de création
- `UpdatedAt` (DateTime) : Date de modification
- `ChampionshipId` (Guid?) : ID du championnat (optionnel)

**Relations** :
- `Championship` : Championnat auquel appartient la partie (optionnel)
- `GamePlayers` : Joueurs participant à cette partie
- `Deals` : Donnes de cette partie

**Règles métier** :
- Une partie à 3 joueurs = 21 donnes (7 contrats × 3 joueurs)
- Une partie à 4 joueurs = 28 donnes (7 contrats × 4 joueurs)
- Chaque joueur déclare exactement 7 contrats (un de chaque type)

**Index** :
- Index sur `Status` pour filtrage
- Index sur `ChampionshipId` pour requêtes par championnat

---

### GamePlayer

Table de jonction entre `Game` et `Player` avec informations spécifiques à la participation.

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `GameId` (Guid) : ID de la partie
- `PlayerId` (Guid) : ID du joueur
- `Position` (int) : Position dans la partie (0 à 3)
- `TotalScore` (int) : Score total accumulé
- `RemainingChallenges` (int) : Nombre de contres restants
- `RemainingSurcontres` (int) : Nombre de surcontres restants

**Relations** :
- `Game` : La partie (cascade delete)
- `Player` : Le joueur (restrict delete)
- `DeclaredDeals` : Donnes où ce joueur est déclarant

**Règles métier** :
- `Position` détermine l'ordre de jeu et le déclarant initial
- `RemainingChallenges` initialisé à (nombre de joueurs - 1) × 7
- Chaque contre utilisé décrémente `RemainingChallenges`

**Contraintes** :
- Unique (`GameId`, `PlayerId`) : un joueur ne peut participer qu'une fois à une partie
- Unique (`GameId`, `Position`) : chaque position est unique dans une partie

---

### Deal

Représente une donne (un tour de jeu avec un contrat spécifique).

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `GameId` (Guid) : ID de la partie
- `DealNumber` (int) : Numéro de la donne (1 à 21 ou 28)
- `DeclarerGamePlayerId` (Guid) : ID du `GamePlayer` déclarant
- `ContractType` (ContractType) : Type de contrat choisi
- `IsCompleted` (bool) : Indique si la donne est terminée
- `StartedAt` (DateTime) : Date de début
- `CompletedAt` (DateTime?) : Date de fin

**Relations** :
- `Game` : La partie (cascade delete)
- `Declarer` : Le joueur déclarant (restrict delete)
- `DealScores` : Scores de chaque joueur pour cette donne
- `Challenges` : Contres et surcontres pour cette donne

**Règles métier** :
- Le déclarant tourne selon la position : `DealNumber % PlayerCount`
- Chaque contrat ne peut être joué qu'une fois par joueur dans une partie
- Les contres doivent être déclarés avant le début de la donne

**Contraintes** :
- Unique (`GameId`, `DealNumber`) : chaque donne a un numéro unique dans une partie

---

### DealScore

Représente le score d'un joueur pour une donne spécifique.

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `DealId` (Guid) : ID de la donne
- `GamePlayerId` (Guid) : ID du `GamePlayer`
- `BaseScore` (int) : Score de base (avant contres/surcontres)
- `FinalScore` (int) : Score final (après contres/surcontres)
- `ScoreDetails` (string?, max 500) : Détails du calcul (ex: "3 plis × 5 = 15")

**Relations** :
- `Deal` : La donne (cascade delete)
- `GamePlayer` : Le joueur (restrict delete)

**Règles de calcul** :

| Contrat | Calcul du BaseScore |
|---------|---------------------|
| Barbu | -20 si Roi de Cœur pris, 0 sinon |
| NoPlis | Nombre de plis × -2 |
| Coeurs | (Nombre de cœurs × -2) + (As de cœur × -6 supplémentaires) |
| Dames | Nombre de Dames × -6 |
| DeuxDerniersPlis | -10 si avant-dernier pli, -20 si dernier pli |
| Atout | Nombre de plis × +5 |
| Reussite | +45 (1er), +20 (2e), +10 (3e), -10 (4e) |

**Contraintes** :
- Unique (`DealId`, `GamePlayerId`) : un score par joueur par donne

---

### Challenge

Représente un contre ou surcontre entre deux joueurs pour une donne.

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `DealId` (Guid) : ID de la donne
- `Type` (ChallengeType) : Contre ou Surcontre
- `ChallengerGamePlayerId` (Guid) : ID du joueur qui lance le défi
- `ChallengedGamePlayerId` (Guid) : ID du joueur défié
- `ScoreDifference` (int) : Écart de score (valeur absolue)
- `PointsTransferred` (int) : Points transférés

**Relations** :
- `Deal` : La donne (cascade delete)
- `Challenger` : Le joueur qui lance le défi (restrict delete)
- `Challenged` : Le joueur défié (restrict delete)

**Règles de calcul** :

```
écart = |BaseScore(Challenger) - BaseScore(Challenged)|

Si Contre :
  PointsTransferred = écart

Si Surcontre :
  PointsTransferred = écart × 2

Meilleur score : +PointsTransferred
Moins bon score : -PointsTransferred
```

**Règles spéciales** :
- Pour **Atout** et **Réussite** : seul le déclarant peut être contré par les flancs
- Les surcontres ne sont possibles que si le déclarant a été contré
- Un joueur ne peut pas se contrer lui-même

**Contraintes** :
- Unique (`DealId`, `ChallengerGamePlayerId`, `ChallengedGamePlayerId`) : un seul contre entre 2 joueurs par donne

---

### Championship

Représente un championnat (groupe de joueurs + plusieurs parties).

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `Name` (string, required, max 200) : Nom du championnat
- `Description` (string?, max 1000) : Description optionnelle
- `StartDate` (DateTime) : Date de début
- `EndDate` (DateTime?) : Date de fin (optionnelle)
- `IsActive` (bool) : Indique si le championnat est actif
- `CreatedAt` (DateTime) : Date de création
- `UpdatedAt` (DateTime) : Date de modification

**Relations** :
- `ChampionshipPlayers` : Joueurs participant au championnat
- `Games` : Parties du championnat

**Index** :
- Index sur `Name` pour recherche
- Index sur `IsActive` pour filtrage

---

### ChampionshipPlayer

Table de jonction entre `Championship` et `Player` avec points de championnat.

**Propriétés** :
- `Id` (Guid) : Identifiant unique
- `ChampionshipId` (Guid) : ID du championnat
- `PlayerId` (Guid) : ID du joueur
- `TotalPoints` (decimal, précision 10,1) : Points totaux accumulés
- `GamesPlayed` (int) : Nombre de parties jouées
- `JoinedAt` (DateTime) : Date d'inscription

**Relations** :
- `Championship` : Le championnat (cascade delete)
- `Player` : Le joueur (restrict delete)

**Règles de calcul des points** :

**Pour une partie à 4 joueurs** :
```
Base selon rang : 1er=10, 2e=5, 3e=2, 4e=0
Bonus = (ScorePartie / 10) arrondi à 0,5
PointsChampionnat = Base + Bonus
```

**Pour une partie à 3 joueurs** :
```
Base selon rang : 1er=8, 2e=3, 3e=0
Bonus = (ScorePartie / 10) arrondi à 0,5
PointsChampionnat = Base + Bonus
```

**Exemple** :
Partie à 4 joueurs, scores finaux : 85, 45, -30, -100
- 1er (85) : 10 + 8.5 = 18.5 points
- 2e (45) : 5 + 4.5 = 9.5 points
- 3e (-30) : 2 - 3.0 = -1.0 points
- 4e (-100) : 0 - 10.0 = -10.0 points

**Contraintes** :
- Unique (`ChampionshipId`, `PlayerId`) : un joueur ne peut participer qu'une fois à un championnat

## Diagramme des relations

```
Player
  ├─── GamePlayers (1:N)
  │      └─── Game
  │             ├─── Deals (1:N)
  │             │      ├─── DealScores (1:N)
  │             │      └─── Challenges (1:N)
  │             └─── Championship (N:1, optionnel)
  │
  └─── ChampionshipPlayers (1:N)
         └─── Championship
                └─── Games (1:N)
```

## Flux de jeu typique

1. **Création d'une partie** :
   - Créer `Game` avec `PlayerCount` = 3 ou 4
   - Créer `GamePlayer` pour chaque joueur (positions 0 à 2 ou 3)
   - Initialiser `RemainingChallenges` pour chaque joueur

2. **Pour chaque donne** :
   - Créer `Deal` avec le déclarant (rotation selon position)
   - Le déclarant choisit un `ContractType` non encore joué
   - Les joueurs déclarent leurs `Challenge` (contres)
   - Jouer la donne (logique de jeu hors scope du domaine)
   - Créer `DealScore` pour chaque joueur avec `BaseScore`
   - Appliquer les `Challenge` pour calculer `FinalScore`
   - Mettre à jour `TotalScore` de chaque `GamePlayer`

3. **Fin de partie** :
   - Marquer `Game.Status` = `Completed`
   - Calculer le classement final
   - Si championnat : calculer et ajouter les points au `ChampionshipPlayer`
   - Mettre à jour les stats du `Player` (GamesPlayed, Wins)

## Validation des invariants

Les règles métier à valider lors des opérations :

- Un joueur ne peut pas participer 2 fois à la même partie
- Le nombre de joueurs doit être 3 ou 4
- Chaque joueur ne peut déclarer qu'un seul contrat de chaque type
- Un joueur ne peut pas se contrer lui-même
- Pour Atout/Réussite, seuls les flancs peuvent contrer le déclarant
- Le nombre de contres ne peut pas dépasser `RemainingChallenges`
- Une donne ne peut pas être complétée sans scores pour tous les joueurs

## Notes d'implémentation

- Tous les ID sont des `Guid` pour faciliter la distribution et éviter les collisions
- Les dates utilisent `DateTime` (à convertir en UTC côté application)
- Les relations `Restrict` empêchent la suppression accidentelle de données référencées
- Les relations `Cascade` permettent le nettoyage automatique des données dépendantes
- Les index sont placés sur les colonnes fréquemment utilisées dans les requêtes
