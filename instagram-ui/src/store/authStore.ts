import LoginResponse from "@/models/LoginResponse";
import { defineStore } from "pinia";
import { Ref, ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
    const isAuthorized = ref(false)
    const authInfo: Ref<LoginResponse> = ref({
        userId: 0,
        authToken: '',
        email: ''
    })

    const authorize = (response: LoginResponse) => {
        if (!response.authToken) {
            isAuthorized.value = false
        } else {
            authInfo.value = response
            isAuthorized.value = true
        }

        return isAuthorized.value
    }

    return { isAuthorized, authorize }
})  