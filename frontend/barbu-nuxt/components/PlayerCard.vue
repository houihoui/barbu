<template>
  <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
    <div class="flex items-center space-x-4">
      <img
        v-if="player.avatar"
        :src="player.avatar"
        :alt="player.name"
        class="w-16 h-16 rounded-full"
      />
      <div v-else class="w-16 h-16 rounded-full bg-indigo-100 flex items-center justify-center">
        <span class="text-2xl font-bold text-indigo-600">{{ player.name.charAt(0).toUpperCase() }}</span>
      </div>

      <div class="flex-1">
        <h3 class="text-lg font-semibold text-gray-900">{{ player.name }}</h3>
        <div class="mt-1 flex gap-4 text-sm text-gray-600">
          <span>{{ player.gamesPlayed }} parties</span>
          <span>{{ player.wins }} victoires</span>
          <span v-if="player.gamesPlayed > 0" class="text-indigo-600 font-medium">
            {{ winRate }}% victoires
          </span>
        </div>
      </div>

      <div class="flex gap-2">
        <button
          @click="$emit('edit', player)"
          class="p-2 text-indigo-600 hover:bg-indigo-50 rounded-md transition-colors"
          title="Modifier"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
        </button>
        <button
          @click="$emit('delete', player)"
          class="p-2 text-red-600 hover:bg-red-50 rounded-md transition-colors"
          title="Supprimer"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Player } from '../types'

const props = defineProps<{
  player: Player
}>()

defineEmits<{
  edit: [player: Player]
  delete: [player: Player]
}>()

const winRate = computed(() => {
  if (props.player.gamesPlayed === 0) return 0
  return Math.round((props.player.wins / props.player.gamesPlayed) * 100)
})
</script>
