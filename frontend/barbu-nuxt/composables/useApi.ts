import type { Player, CreatePlayer, UpdatePlayer, Game, CreateGame, Championship, CreateChampionship, UpdateChampionship } from '../types'

const API_BASE_URL = 'http://localhost:5000/api'

export const useApi = () => {
  // Players
  const getPlayers = async (): Promise<Player[]> => {
    return await $fetch(`${API_BASE_URL}/players`)
  }

  const getPlayer = async (id: string): Promise<Player> => {
    return await $fetch(`${API_BASE_URL}/players/${id}`)
  }

  const createPlayer = async (player: CreatePlayer): Promise<Player> => {
    return await $fetch(`${API_BASE_URL}/players`, {
      method: 'POST',
      body: player
    })
  }

  const updatePlayer = async (id: string, player: UpdatePlayer): Promise<Player> => {
    return await $fetch(`${API_BASE_URL}/players/${id}`, {
      method: 'PUT',
      body: player
    })
  }

  const deletePlayer = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/players/${id}`, {
      method: 'DELETE'
    })
  }

  // Games
  const getGames = async (): Promise<Game[]> => {
    return await $fetch(`${API_BASE_URL}/games`)
  }

  const getGame = async (id: string): Promise<Game> => {
    return await $fetch(`${API_BASE_URL}/games/${id}`)
  }

  const createGame = async (game: CreateGame): Promise<Game> => {
    return await $fetch(`${API_BASE_URL}/games`, {
      method: 'POST',
      body: game
    })
  }

  const startGame = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/games/${id}/start`, {
      method: 'POST'
    })
  }

  const completeGame = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/games/${id}/complete`, {
      method: 'POST'
    })
  }

  const deleteGame = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/games/${id}`, {
      method: 'DELETE'
    })
  }

  // Championships
  const getChampionships = async (): Promise<Championship[]> => {
    return await $fetch(`${API_BASE_URL}/championships`)
  }

  const getChampionship = async (id: string): Promise<Championship> => {
    return await $fetch(`${API_BASE_URL}/championships/${id}`)
  }

  const createChampionship = async (championship: CreateChampionship): Promise<Championship> => {
    return await $fetch(`${API_BASE_URL}/championships`, {
      method: 'POST',
      body: championship
    })
  }

  const updateChampionship = async (id: string, championship: UpdateChampionship): Promise<Championship> => {
    return await $fetch(`${API_BASE_URL}/championships/${id}`, {
      method: 'PUT',
      body: championship
    })
  }

  const startChampionship = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/championships/${id}/start`, {
      method: 'POST'
    })
  }

  const completeChampionship = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/championships/${id}/complete`, {
      method: 'POST'
    })
  }

  const addPlayerToChampionship = async (championshipId: string, playerId: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/championships/${championshipId}/players/${playerId}`, {
      method: 'POST'
    })
  }

  const removePlayerFromChampionship = async (championshipId: string, playerId: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/championships/${championshipId}/players/${playerId}`, {
      method: 'DELETE'
    })
  }

  const deleteChampionship = async (id: string): Promise<void> => {
    return await $fetch(`${API_BASE_URL}/championships/${id}`, {
      method: 'DELETE'
    })
  }

  return {
    // Players
    getPlayers,
    getPlayer,
    createPlayer,
    updatePlayer,
    deletePlayer,
    // Games
    getGames,
    getGame,
    createGame,
    startGame,
    completeGame,
    deleteGame,
    // Championships
    getChampionships,
    getChampionship,
    createChampionship,
    updateChampionship,
    startChampionship,
    completeChampionship,
    addPlayerToChampionship,
    removePlayerFromChampionship,
    deleteChampionship
  }
}
