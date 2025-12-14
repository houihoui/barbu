<template>
  <div class="bg-white rounded-lg shadow-md overflow-hidden">
    <!-- Header -->
    <div class="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
      <div class="flex items-center gap-3">
        <h3 class="text-lg font-semibold text-gray-900">
          {{ game.name || 'Partie sans nom' }}
        </h3>
        <span
          class="px-3 py-1 text-xs font-semibold rounded-full"
          :class="statusBadgeClass"
        >
          {{ statusText }}
        </span>
      </div>
      <div class="text-sm text-gray-500">
        {{ formatDate(game.createdAt) }}
      </div>
    </div>

    <!-- Body - Joueurs -->
    <div class="px-6 py-4">
      <div class="grid grid-cols-2 sm:grid-cols-4 gap-3">
        <div
          v-for="player in sortedPlayers"
          :key="player.id"
          class="flex items-center gap-2 p-2 bg-gray-50 rounded-lg"
        >
          <div class="relative flex-shrink-0">
            <img
              :src="player.playerAvatar || `https://api.dicebear.com/7.x/avataaars/svg?seed=${player.playerName}`"
              :alt="player.playerName"
              class="w-10 h-10 rounded-full border-2 border-gray-300"
            />
            <div class="absolute -top-1 -left-1 w-5 h-5 bg-indigo-600 text-white rounded-full flex items-center justify-center text-xs font-bold">
              {{ player.position }}
            </div>
          </div>
          <div class="flex-1 min-w-0">
            <div class="text-sm font-medium text-gray-900 truncate">
              {{ player.playerName }}
            </div>
            <div
              class="text-xs font-semibold"
              :class="player.totalScore <= 0 ? 'text-green-600' : 'text-red-600'"
            >
              {{ player.totalScore }} pts
            </div>
          </div>
        </div>
      </div>

      <!-- Progression -->
      <div v-if="game.status === GameStatus.InProgress" class="mt-4 pt-4 border-t border-gray-200">
        <div class="flex items-center justify-between text-sm">
          <span class="text-gray-600">Progression</span>
          <span class="font-semibold text-indigo-600">
            Donne {{ game.currentDealNumber }} / {{ maxDeals }}
          </span>
        </div>
        <div class="mt-2 w-full bg-gray-200 rounded-full h-2">
          <div
            class="bg-indigo-600 h-2 rounded-full transition-all"
            :style="{ width: `${(game.currentDealNumber / maxDeals) * 100}%` }"
          ></div>
        </div>
      </div>
    </div>

    <!-- Footer - Actions -->
    <div class="px-6 py-4 bg-gray-50 border-t border-gray-200 flex gap-2 justify-end">
      <!-- Pending -->
      <template v-if="game.status === GameStatus.Pending">
        <button
          @click="$emit('start', game.id)"
          class="px-4 py-2 bg-indigo-600 text-white text-sm font-semibold rounded-md hover:bg-indigo-700 transition-colors"
        >
          Démarrer
        </button>
        <button
          @click="$emit('delete', game.id)"
          class="px-4 py-2 bg-red-600 text-white text-sm font-semibold rounded-md hover:bg-red-700 transition-colors"
        >
          Supprimer
        </button>
      </template>

      <!-- InProgress -->
      <template v-else-if="game.status === GameStatus.InProgress">
        <button
          @click="$emit('view', game.id)"
          class="px-4 py-2 bg-indigo-600 text-white text-sm font-semibold rounded-md hover:bg-indigo-700 transition-colors"
        >
          Reprendre
        </button>
      </template>

      <!-- Completed -->
      <template v-else-if="game.status === GameStatus.Completed">
        <button
          @click="$emit('view', game.id)"
          class="px-4 py-2 bg-gray-600 text-white text-sm font-semibold rounded-md hover:bg-gray-700 transition-colors"
        >
          Voir détails
        </button>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Game } from '../types'
import { GameStatus } from '../types'

const props = defineProps<{
  game: Game
}>()

defineEmits<{
  start: [id: string]
  delete: [id: string]
  view: [id: string]
}>()

const sortedPlayers = computed(() =>
  [...props.game.players].sort((a, b) => a.position - b.position)
)

const maxDeals = computed(() =>
  props.game.playerCount === 3 ? 21 : 28
)

const statusText = computed(() => {
  switch (props.game.status) {
    case GameStatus.Pending:
      return 'En attente'
    case GameStatus.InProgress:
      return 'En cours'
    case GameStatus.Completed:
      return 'Terminée'
    case GameStatus.Abandoned:
      return 'Abandonnée'
    default:
      return 'Inconnu'
  }
})

const statusBadgeClass = computed(() => {
  switch (props.game.status) {
    case GameStatus.Pending:
      return 'bg-yellow-100 text-yellow-800'
    case GameStatus.InProgress:
      return 'bg-blue-100 text-blue-800'
    case GameStatus.Completed:
      return 'bg-green-100 text-green-800'
    case GameStatus.Abandoned:
      return 'bg-gray-100 text-gray-800'
    default:
      return 'bg-gray-100 text-gray-800'
  }
})

const formatDate = (dateString: string) => {
  const date = new Date(dateString)
  const now = new Date()
  const diffMs = now.getTime() - date.getTime()
  const diffMins = Math.floor(diffMs / 60000)
  const diffHours = Math.floor(diffMs / 3600000)
  const diffDays = Math.floor(diffMs / 86400000)

  if (diffMins < 1) return 'À l\'instant'
  if (diffMins < 60) return `Il y a ${diffMins} min`
  if (diffHours < 24) return `Il y a ${diffHours}h`
  if (diffDays < 7) return `Il y a ${diffDays}j`

  return date.toLocaleDateString('fr-FR', {
    day: 'numeric',
    month: 'short',
    year: date.getFullYear() !== now.getFullYear() ? 'numeric' : undefined
  })
}
</script>
