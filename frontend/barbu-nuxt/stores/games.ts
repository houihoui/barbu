import { defineStore } from 'pinia'
import type { Game, GameDetails, CreateGame } from '../types'

const API_BASE_URL = 'http://localhost:5000/api'

export const useGamesStore = defineStore('games', () => {
  // State
  const games = ref<Game[]>([])
  const currentGame = ref<Game | null>(null)
  const currentGameDetails = ref<GameDetails | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Actions
  const fetchGames = async () => {
    loading.value = true
    error.value = null
    try {
      games.value = await $fetch(`${API_BASE_URL}/games`)
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du chargement des parties'
      throw e
    } finally {
      loading.value = false
    }
  }

  const fetchGame = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      currentGame.value = await $fetch(`${API_BASE_URL}/games/${id}`)
      return currentGame.value
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du chargement de la partie'
      throw e
    } finally {
      loading.value = false
    }
  }

  const fetchGameDetails = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      currentGameDetails.value = await $fetch(`${API_BASE_URL}/games/${id}/details`)
      return currentGameDetails.value
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du chargement des détails'
      throw e
    } finally {
      loading.value = false
    }
  }

  const createGame = async (game: CreateGame) => {
    loading.value = true
    error.value = null
    try {
      const newGame = await $fetch(`${API_BASE_URL}/games`, {
        method: 'POST',
        body: game
      })
      games.value.unshift(newGame) // Ajouter en tête
      return newGame
    } catch (e: any) {
      error.value = e.message || 'Erreur lors de la création de la partie'
      throw e
    } finally {
      loading.value = false
    }
  }

  const startGame = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      await $fetch(`${API_BASE_URL}/games/${id}/start`, { method: 'POST' })
      // Mettre à jour le statut local
      const game = games.value.find(g => g.id === id)
      if (game) game.status = 1 // InProgress
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du démarrage de la partie'
      throw e
    } finally {
      loading.value = false
    }
  }

  const deleteGame = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      await $fetch(`${API_BASE_URL}/games/${id}`, { method: 'DELETE' })
      games.value = games.value.filter(g => g.id !== id)
    } catch (e: any) {
      error.value = e.message || 'Erreur lors de la suppression'
      throw e
    } finally {
      loading.value = false
    }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    games, currentGame, currentGameDetails, loading, error,
    fetchGames, fetchGame, fetchGameDetails,
    createGame, startGame, deleteGame, clearError
  }
})
