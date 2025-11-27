<template>
  <div class="px-4 py-6 sm:px-0">
    <!-- Header -->
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold text-gray-900">Gestion des Joueurs</h2>
      <button
        @click="showForm = !showForm"
        class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 transition-colors flex items-center gap-2"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        {{ showForm ? 'Masquer le formulaire' : 'Ajouter un joueur' }}
      </button>
    </div>

    <!-- Error message -->
    <div
      v-if="playersStore.error"
      class="mb-4 bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md flex justify-between items-center"
    >
      <span>{{ playersStore.error }}</span>
      <button
        @click="playersStore.clearError()"
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

    <!-- Player Form -->
    <div v-if="showForm" class="mb-6">
      <PlayerForm
        :player="editingPlayer || undefined"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>

    <!-- Loading state -->
    <div v-if="playersStore.loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
    </div>

    <!-- Players list -->
    <div v-else-if="playersStore.players.length > 0" class="space-y-4">
      <PlayerCard
        v-for="player in playersStore.players"
        :key="player.id"
        :player="player"
        @edit="handleEdit"
        @delete="handleDelete"
      />
    </div>

    <!-- Empty state -->
    <div v-else class="bg-white shadow sm:rounded-lg p-12 text-center">
      <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
      </svg>
      <h3 class="mt-2 text-lg font-medium text-gray-900">Aucun joueur</h3>
      <p class="mt-1 text-sm text-gray-500">Commencez par ajouter votre premier joueur.</p>
      <div class="mt-6">
        <button
          @click="showForm = true"
          class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 transition-colors"
        >
          Ajouter un joueur
        </button>
      </div>
    </div>

    <!-- Delete confirmation modal -->
    <div
      v-if="deletingPlayer"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      @click.self="deletingPlayer = null"
    >
      <div class="bg-white rounded-lg p-6 max-w-md w-full mx-4">
        <h3 class="text-lg font-semibold text-gray-900 mb-2">Confirmer la suppression</h3>
        <p class="text-gray-600 mb-6">
          Êtes-vous sûr de vouloir supprimer le joueur <strong>{{ deletingPlayer.name }}</strong> ?
          Cette action est irréversible.
        </p>
        <div class="flex gap-3">
          <button
            @click="confirmDelete"
            class="flex-1 bg-red-600 text-white py-2 px-4 rounded-md hover:bg-red-700 transition-colors"
          >
            Supprimer
          </button>
          <button
            @click="deletingPlayer = null"
            class="flex-1 bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400 transition-colors"
          >
            Annuler
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Player, CreatePlayer, UpdatePlayer } from '../../types'
import { usePlayersStore } from '../../stores/players'

definePageMeta({
  title: 'Joueurs'
})

const playersStore = usePlayersStore()
const showForm = ref(false)
const editingPlayer = ref<Player | null>(null)
const deletingPlayer = ref<Player | null>(null)
const successMessage = ref('')

// Load players on mount
onMounted(async () => {
  await playersStore.fetchPlayers()
})

const handleSubmit = async (data: CreatePlayer | UpdatePlayer) => {
  try {
    if (editingPlayer.value) {
      await playersStore.updatePlayer(editingPlayer.value.id, data as UpdatePlayer)
      successMessage.value = 'Joueur modifié avec succès'
      editingPlayer.value = null
      showForm.value = false
    } else {
      await playersStore.addPlayer(data as CreatePlayer)
      successMessage.value = 'Joueur ajouté avec succès'
    }
    setTimeout(() => successMessage.value = '', 3000)
  } catch (error) {
    console.error('Error submitting player:', error)
  }
}

const handleEdit = (player: Player) => {
  editingPlayer.value = player
  showForm.value = true
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const handleCancel = () => {
  editingPlayer.value = null
  showForm.value = false
}

const handleDelete = (player: Player) => {
  deletingPlayer.value = player
}

const confirmDelete = async () => {
  if (!deletingPlayer.value) return

  try {
    await playersStore.removePlayer(deletingPlayer.value.id)
    successMessage.value = 'Joueur supprimé avec succès'
    deletingPlayer.value = null
    setTimeout(() => successMessage.value = '', 3000)
  } catch (error) {
    console.error('Error deleting player:', error)
    deletingPlayer.value = null
  }
}
</script>
