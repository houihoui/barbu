import { defineStore } from 'pinia'
import type { Player, CreatePlayer, UpdatePlayer } from '../types'

const API_BASE_URL = 'http://localhost:5000/api'

export const usePlayersStore = defineStore('players', () => {
  // State
  const players = ref<Player[]>([])
  const currentPlayer = ref<Player | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Actions
  const fetchPlayers = async () => {
    loading.value = true
    error.value = null
    try {
      players.value = await $fetch(`${API_BASE_URL}/players`)
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du chargement des joueurs'
      throw e
    } finally {
      loading.value = false
    }
  }

  const fetchPlayer = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      currentPlayer.value = await $fetch(`${API_BASE_URL}/players/${id}`)
      return currentPlayer.value
    } catch (e: any) {
      error.value = e.message || 'Erreur lors du chargement du joueur'
      throw e
    } finally {
      loading.value = false
    }
  }

  const addPlayer = async (player: CreatePlayer) => {
    loading.value = true
    error.value = null
    try {
      const newPlayer = await $fetch(`${API_BASE_URL}/players`, {
        method: 'POST',
        body: player
      })
      players.value.push(newPlayer)
      return newPlayer
    } catch (e: any) {
      error.value = e.message || 'Erreur lors de la création du joueur'
      throw e
    } finally {
      loading.value = false
    }
  }

  const updatePlayer = async (id: string, player: UpdatePlayer) => {
    loading.value = true
    error.value = null
    try {
      const updatedPlayer = await $fetch(`${API_BASE_URL}/players/${id}`, {
        method: 'PUT',
        body: player
      })
      const index = players.value.findIndex(p => p.id === id)
      if (index !== -1) {
        players.value[index] = updatedPlayer
      }
      return updatedPlayer
    } catch (e: any) {
      error.value = e.message || 'Erreur lors de la mise à jour du joueur'
      throw e
    } finally {
      loading.value = false
    }
  }

  const removePlayer = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      await $fetch(`${API_BASE_URL}/players/${id}`, {
        method: 'DELETE'
      })
      players.value = players.value.filter(p => p.id !== id)
    } catch (e: any) {
      error.value = e.message || 'Erreur lors de la suppression du joueur'
      throw e
    } finally {
      loading.value = false
    }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    // State
    players,
    currentPlayer,
    loading,
    error,
    // Actions
    fetchPlayers,
    fetchPlayer,
    addPlayer,
    updatePlayer,
    removePlayer,
    clearError
  }
})
