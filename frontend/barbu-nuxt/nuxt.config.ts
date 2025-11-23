// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },

  modules: [
    '@pinia/nuxt',
    '@nuxtjs/tailwindcss'
  ],

  typescript: {
    strict: true,
    typeCheck: false  // Désactivé pour l'instant, activer avec vue-tsc si besoin
  },

  app: {
    head: {
      title: 'Le Barbu - Gestion de parties',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'description', content: 'Application de gestion du jeu de cartes Le Barbu' }
      ]
    }
  }
})
