<template>
  <div class="px-4 py-6 sm:px-0">
    <div class="bg-white shadow sm:rounded-lg p-6">
      <h2 class="text-2xl font-bold text-gray-900 mb-4">Historique des Parties</h2>
      <p class="text-gray-600">
        Liste des parties (en cours et terminées), possibilité de reprendre ou supprimer une partie.
      </p>
      <div class="mt-6">
        <p class="text-sm text-gray-500">
          À implémenter : liste filtrable des parties, statut (en cours/terminée),
          boutons pour reprendre/voir détails/supprimer
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  title: 'Historique'
})
</script>
