import { defineStore } from "pinia";
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
    const authorized = ref(false)

    return { authorized }
})  