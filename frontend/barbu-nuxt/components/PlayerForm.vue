<template>
  <div class="bg-white rounded-lg shadow-md p-6">
    <h3 class="text-lg font-semibold text-gray-900 mb-4">
      {{ isEditing ? 'Modifier un joueur' : 'Ajouter un joueur' }}
    </h3>

    <form @submit.prevent="handleSubmit" class="space-y-4">
      <div>
        <label for="name" class="block text-sm font-medium text-gray-700 mb-1">
          Nom du joueur *
        </label>
        <input
          id="name"
          v-model="formData.name"
          type="text"
          required
          minlength="2"
          maxlength="100"
          class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="Entrez le nom du joueur"
        />
      </div>

      <div>
        <label for="avatar" class="block text-sm font-medium text-gray-700 mb-1">
          URL de l'avatar (optionnel)
        </label>
        <input
          id="avatar"
          v-model="formData.avatar"
          type="url"
          maxlength="500"
          class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="https://example.com/avatar.jpg"
        />
        <p class="mt-1 text-xs text-gray-500">
          Ou générer un avatar avec
          <a
            href="#"
            @click.prevent="generateAvatar"
            class="text-indigo-600 hover:text-indigo-800"
          >
            DiceBear
          </a>
        </p>
      </div>

      <div v-if="formData.avatar" class="flex justify-center">
        <img
          :src="formData.avatar"
          :alt="formData.name || 'Aperçu'"
          class="w-20 h-20 rounded-full border-2 border-gray-200"
        />
      </div>

      <div class="flex gap-3 pt-2">
        <button
          type="submit"
          :disabled="!formData.name.trim()"
          class="flex-1 bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          {{ isEditing ? 'Modifier' : 'Ajouter' }}
        </button>
        <button
          v-if="isEditing"
          type="button"
          @click="handleCancel"
          class="flex-1 bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400 transition-colors"
        >
          Annuler
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import type { Player, CreatePlayer, UpdatePlayer } from '../types'

const props = defineProps<{
  player?: Player
}>()

const emit = defineEmits<{
  submit: [data: CreatePlayer | UpdatePlayer]
  cancel: []
}>()

const isEditing = computed(() => !!props.player)

const formData = reactive<CreatePlayer>({
  name: props.player?.name || '',
  avatar: props.player?.avatar || ''
})

watch(() => props.player, (newPlayer) => {
  if (newPlayer) {
    formData.name = newPlayer.name
    formData.avatar = newPlayer.avatar || ''
  } else {
    formData.name = ''
    formData.avatar = ''
  }
})

const generateAvatar = () => {
  const seed = formData.name || 'random-' + Date.now()
  formData.avatar = `https://api.dicebear.com/7.x/avataaars/svg?seed=${encodeURIComponent(seed)}`
}

const handleSubmit = () => {
  const data = {
    name: formData.name.trim(),
    avatar: formData.avatar?.trim() || undefined
  }
  emit('submit', data)

  if (!isEditing.value) {
    // Reset form after adding
    formData.name = ''
    formData.avatar = ''
  }
}

const handleCancel = () => {
  formData.name = ''
  formData.avatar = ''
  emit('cancel')
}
</script>
