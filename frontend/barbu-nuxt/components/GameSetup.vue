<template>
  <div class="bg-white rounded-lg shadow-md p-6">
    <h3 class="text-xl font-semibold text-gray-900 mb-6">Configuration de la partie</h3>

    <form @submit.prevent="handleSubmit" class="space-y-6">
      <!-- Nom de la partie (optionnel) -->
      <div>
        <label for="gameName" class="block text-sm font-medium text-gray-700 mb-2">
          Nom de la partie (optionnel)
        </label>
        <input
          id="gameName"
          v-model="formData.name"
          type="text"
          maxlength="200"
          class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
          placeholder="Ma partie entre amis..."
        />
      </div>

      <!-- Nombre de joueurs -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-3">
          Nombre de joueurs
        </label>
        <div class="flex gap-4">
          <label class="flex-1 cursor-pointer">
            <input
              type="radio"
              v-model.number="formData.playerCount"
              :value="3"
              class="sr-only"
            />
            <div
              class="border-2 rounded-lg p-4 text-center transition-all"
              :class="formData.playerCount === 3
                ? 'border-indigo-600 bg-indigo-50 text-indigo-900'
                : 'border-gray-300 hover:border-gray-400 text-gray-700'"
            >
              <div class="text-2xl font-bold">3</div>
              <div class="text-xs mt-1">joueurs (21 donnes)</div>
            </div>
          </label>
          <label class="flex-1 cursor-pointer">
            <input
              type="radio"
              v-model.number="formData.playerCount"
              :value="4"
              class="sr-only"
            />
            <div
              class="border-2 rounded-lg p-4 text-center transition-all"
              :class="formData.playerCount === 4
                ? 'border-indigo-600 bg-indigo-50 text-indigo-900'
                : 'border-gray-300 hover:border-gray-400 text-gray-700'"
            >
              <div class="text-2xl font-bold">4</div>
              <div class="text-xs mt-1">joueurs (28 donnes)</div>
            </div>
          </label>
        </div>
      </div>

      <!-- Sélection des joueurs -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Sélectionnez {{ formData.playerCount }} joueurs
          <span class="text-indigo-600 font-semibold ml-2">
            ({{ formData.playerIds.length }} / {{ formData.playerCount }})
          </span>
        </label>

        <div v-if="playersStore.players.length === 0" class="text-center py-8 text-gray-500">
          Aucun joueur disponible. Créez d'abord des joueurs dans l'onglet "Joueurs".
        </div>

        <div v-else class="grid grid-cols-2 sm:grid-cols-3 gap-3 max-h-96 overflow-y-auto p-1">
          <label
            v-for="player in playersStore.players"
            :key="player.id"
            class="relative cursor-pointer group"
          >
            <input
              type="checkbox"
              :value="player.id"
              v-model="formData.playerIds"
              :disabled="!canSelectMore && !isSelected(player.id)"
              class="sr-only"
            />
            <div
              class="border-2 rounded-lg p-3 transition-all h-full flex flex-col items-center justify-center"
              :class="{
                'border-indigo-600 bg-indigo-50': isSelected(player.id),
                'border-gray-300 hover:border-gray-400': !isSelected(player.id) && canSelectMore,
                'border-gray-200 opacity-50 cursor-not-allowed': !canSelectMore && !isSelected(player.id)
              }"
            >
              <div class="relative">
                <img
                  :src="player.avatar || `https://api.dicebear.com/7.x/avataaars/svg?seed=${player.name}`"
                  :alt="player.name"
                  class="w-16 h-16 rounded-full border-2"
                  :class="isSelected(player.id) ? 'border-indigo-600' : 'border-gray-300'"
                />
                <div
                  v-if="getPlayerPosition(player.id) !== null"
                  class="absolute -top-1 -right-1 w-6 h-6 bg-indigo-600 text-white rounded-full flex items-center justify-center text-xs font-bold shadow-lg"
                >
                  {{ getPlayerPosition(player.id) }}
                </div>
              </div>
              <span
                class="text-sm font-medium mt-2 text-center line-clamp-1"
                :class="isSelected(player.id) ? 'text-indigo-900' : 'text-gray-700'"
              >
                {{ player.name }}
              </span>
            </div>
          </label>
        </div>
      </div>

      <!-- Joueurs sélectionnés - Ordre de jeu -->
      <div v-if="selectedPlayers.length > 0" class="bg-gray-50 rounded-lg p-4">
        <h4 class="text-sm font-semibold text-gray-700 mb-3">Ordre de jeu</h4>
        <div class="flex flex-wrap gap-3">
          <div
            v-for="(player, index) in selectedPlayers"
            :key="player.id"
            class="flex items-center gap-2 bg-white border-2 border-indigo-200 rounded-lg px-3 py-2"
          >
            <div class="w-8 h-8 bg-indigo-600 text-white rounded-full flex items-center justify-center text-sm font-bold">
              {{ index + 1 }}
            </div>
            <img
              :src="player.avatar || `https://api.dicebear.com/7.x/avataaars/svg?seed=${player.name}`"
              :alt="player.name"
              class="w-8 h-8 rounded-full border border-gray-300"
            />
            <span class="text-sm font-medium text-gray-900">{{ player.name }}</span>
          </div>
        </div>
        <p class="text-xs text-gray-500 mt-2">
          L'ordre de sélection détermine les positions des joueurs dans la partie.
        </p>
      </div>

      <!-- Bouton submit -->
      <button
        type="submit"
        :disabled="!isValid || playersStore.loading"
        class="w-full py-3 px-4 rounded-md font-semibold text-white transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        :class="isValid && !playersStore.loading
          ? 'bg-indigo-600 hover:bg-indigo-700'
          : 'bg-gray-400'"
      >
        <span v-if="playersStore.loading">Création en cours...</span>
        <span v-else-if="!isValid">Sélectionnez {{ formData.playerCount }} joueurs</span>
        <span v-else>Créer la partie</span>
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { usePlayersStore } from '../stores/players'
import type { CreateGame } from '../types'

const emit = defineEmits<{ submit: [data: CreateGame] }>()
const playersStore = usePlayersStore()

const formData = reactive<CreateGame>({
  name: '',
  playerCount: 4,
  playerIds: [],
  championshipId: undefined
})

const selectedPlayers = computed(() =>
  formData.playerIds
    .map(id => playersStore.players.find(p => p.id === id))
    .filter((p): p is NonNullable<typeof p> => p !== undefined)
)

const isSelected = (playerId: string) =>
  formData.playerIds.includes(playerId)

const getPlayerPosition = (playerId: string) => {
  const index = formData.playerIds.indexOf(playerId)
  return index === -1 ? null : index + 1
}

const canSelectMore = computed(() =>
  formData.playerIds.length < formData.playerCount
)

const isValid = computed(() =>
  formData.playerIds.length === formData.playerCount
)

// Réinitialiser la sélection si on change le nombre de joueurs
watch(() => formData.playerCount, () => {
  if (formData.playerIds.length > formData.playerCount) {
    formData.playerIds = formData.playerIds.slice(0, formData.playerCount)
  }
})

const handleSubmit = () => {
  if (!isValid.value) return

  emit('submit', {
    name: formData.name?.trim() || undefined,
    playerCount: formData.playerCount,
    playerIds: formData.playerIds,
    championshipId: formData.championshipId
  })

  // Reset form
  formData.name = ''
  formData.playerIds = []
}
</script>
