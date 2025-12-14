<template>
  <div class="px-4 py-6">
    <!-- Header -->
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold text-gray-900">Nouvelle Partie</h2>
    </div>

    <!-- Error message -->
    <div
      v-if="gamesStore.error"
      class="mb-4 bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md flex justify-between items-center"
    >
      <span>{{ gamesStore.error }}</span>
      <button
        @click="gamesStore.clearError()"
        class="text-red-700 hover:text-red-900"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>

    <!-- Success message -->
    <div
      v-if="successMessage"
      class="mb-4 bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-md flex justify-between items-center"
    >
      <span>{{ successMessage }}</span>
      <button
        @click="successMessage = ''"
        class="text-green-700 hover:text-green-900"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>

    <!-- Formulaire de création -->
    <GameSetup @submit="handleSubmit" />

    <!-- Parties récentes -->
    <div v-if="gamesStore.games.length > 0" class="mt-8">
      <h3 class="text-xl font-semibold text-gray-900 mb-4">Parties récentes</h3>

      <!-- Loading state -->
      <div v-if="gamesStore.loading" class="flex justify-center py-8">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>

      <!-- Games list -->
      <div v-else class="space-y-4">
        <GameCard
          v-for="game in recentGames"
          :key="game.id"
          :game="game"
          @start="handleStart"
          @delete="handleDelete"
          @view="handleView"
        />
      </div>
    </div>

    <!-- Empty state -->
    <div v-else-if="!gamesStore.loading" class="mt-8 bg-white shadow rounded-lg p-12 text-center">
      <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
      </svg>
      <h3 class="mt-2 text-lg font-medium text-gray-900">Aucune partie</h3>
      <p class="mt-1 text-sm text-gray-500">Créez votre première partie ci-dessus.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useGamesStore } from '../../stores/games'
import { usePlayersStore } from '../../stores/players'
import GameSetup from '../../components/GameSetup.vue'
import GameCard from '../../components/GameCard.vue'
import type { CreateGame } from '../../types'

definePageMeta({
  title: 'Partie'
})

const gamesStore = useGamesStore()
const playersStore = usePlayersStore()
const successMessage = ref('')

onMounted(async () => {
  console.log('🔵 Page Partie montée, chargement des données...')
  try {
    await playersStore.fetchPlayers() // Pour le formulaire
    await gamesStore.fetchGames()     // Pour la liste
    console.log('✅ Données chargées:', {
      players: playersStore.players.length,
      games: gamesStore.games.length
    })
  } catch (error) {
    console.error('❌ Erreur chargement:', error)
  }
})

const handleSubmit = async (data: CreateGame) => {
  try {
    console.log('🔵 Création de partie:', data)
    await gamesStore.createGame(data)
    successMessage.value = 'Partie créée avec succès !'
    setTimeout(() => successMessage.value = '', 3000)
    console.log('✅ Partie créée')
  } catch (error) {
    console.error('❌ Erreur création partie:', error)
  }
}

const handleStart = async (id: string) => {
  try {
    console.log('🔵 Démarrage partie:', id)
    await gamesStore.startGame(id)
    successMessage.value = 'Partie démarrée !'
    setTimeout(() => successMessage.value = '', 3000)
    // Redirection vers Live (plus tard)
    // navigateTo(`/live?gameId=${id}`)
  } catch (error) {
    console.error('❌ Erreur démarrage:', error)
  }
}

const handleDelete = async (id: string) => {
  if (confirm('Supprimer cette partie ? Cette action est irréversible.')) {
    try {
      console.log('🔵 Suppression partie:', id)
      await gamesStore.deleteGame(id)
      successMessage.value = 'Partie supprimée'
      setTimeout(() => successMessage.value = '', 3000)
    } catch (error) {
      console.error('❌ Erreur suppression:', error)
    }
  }
}

const handleView = (id: string) => {
  console.log('🔵 Affichage partie:', id)
  // Redirection vers Historique ou Live selon le statut (plus tard)
  // navigateTo(`/historique/${id}`)
}

const recentGames = computed(() =>
  gamesStore.games.slice(0, 5)
)
</script>
