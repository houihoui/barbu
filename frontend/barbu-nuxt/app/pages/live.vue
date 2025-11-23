<template>
  <div class="px-4 py-6 sm:px-0">
    <div class="bg-white shadow sm:rounded-lg p-6">
      <h2 class="text-2xl font-bold text-gray-900 mb-4">Vue LIVE</h2>
      <p class="text-gray-600">
        Vue en lecture seule d'une partie en cours : scores actuels, contrats restants, contres restants.
      </p>
      <div class="mt-6">
        <p class="text-sm text-gray-500">
          À implémenter : affichage temps réel des scores, tableau des contrats joués/restants,
          visualisation des contres/surcontres
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  title: 'LIVE'
})
</script>
