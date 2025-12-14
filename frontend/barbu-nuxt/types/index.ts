// Types correspondant aux DTOs de l'API

export enum GameStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
  Abandoned = 3
}

export enum ContractType {
  Barbu = 1,
  NoPlis = 2,
  Coeurs = 3,
  Dames = 4,
  DeuxDerniersPlis = 5,
  Atout = 6,
  Reussite = 7
}

export enum ChallengeType {
  Contre = 1,
  Surcontre = 2
}

// Players
export interface Player {
  id: string
  name: string
  avatar?: string
  gamesPlayed: number
  wins: number
  createdAt: string
  updatedAt: string
}

export interface CreatePlayer {
  name: string
  avatar?: string
}

export interface UpdatePlayer {
  name: string
  avatar?: string
}

// Games
export interface GamePlayer {
  id: string
  playerId: string
  playerName: string
  playerAvatar?: string
  position: number
  totalScore: number
  remainingChallenges: number
  remainingSurcontres: number
}

export interface Game {
  id: string
  name?: string
  playerCount: number
  status: GameStatus
  currentDealNumber: number
  championshipId?: string
  createdAt: string
  startedAt?: string
  completedAt?: string
  players: GamePlayer[]
}

export interface CreateGame {
  name?: string
  playerCount: number
  playerIds: string[]
  championshipId?: string
}

// Championships
export interface ChampionshipPlayer {
  id: string
  playerId: string
  playerName: string
  playerAvatar?: string
  totalPoints: number
  gamesPlayed: number
  ranking: number
}

export interface Championship {
  id: string
  name: string
  description?: string
  isActive: boolean
  createdAt: string
  startedAt?: string
  completedAt?: string
  players: ChampionshipPlayer[]
  totalGames: number
}

export interface CreateChampionship {
  name: string
  description?: string
  playerIds: string[]
}

export interface UpdateChampionship {
  name: string
  description?: string
}

// Deals
export interface DealScore {
  id: string
  dealId: string
  gamePlayerId: string
  playerName: string
  baseScore: number
  finalScore: number
  scoreDetails?: string
}

export interface Challenge {
  id: string
  dealId: string
  type: ChallengeType
  challengerGamePlayerId: string
  challengerPlayerName: string
  challengedGamePlayerId: string
  challengedPlayerName: string
  scoreDifference: number
  pointsTransferred: number
}

export interface Deal {
  id: string
  gameId: string
  dealNumber: number
  declarerGamePlayerId: string
  declarerPlayerName: string
  contractType: ContractType
  isCompleted: boolean
  startedAt: string
  completedAt?: string
  scores: DealScore[]
  challenges: Challenge[]
}

export interface GameDetails extends Game {
  deals: Deal[]
}
