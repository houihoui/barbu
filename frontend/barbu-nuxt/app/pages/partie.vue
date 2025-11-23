<template>
  <div class="px-4 py-6 sm:px-0">
    <div class="bg-white shadow sm:rounded-lg p-6">
      <h2 class="text-2xl font-bold text-gray-900 mb-4">Gestion de Partie</h2>
      <p class="text-gray-600">
        Cette page permettra de créer et gérer une partie de Barbu (3 ou 4 joueurs, 7 contrats par joueur).
      </p>
      <div class="mt-6">
        <p class="text-sm text-gray-500">
          À implémenter : sélection joueurs, gestion des donnes, choix des contrats,
          saisie des scores, gestion des contres/surcontres
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  title: 'Partie'
})
</script>
