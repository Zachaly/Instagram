import LoginResponse from "@/models/LoginResponse";
import { RefSymbol } from "@vue/reactivity";
import axios from "axios";
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
            axios.defaults.headers.common.Authorization = `Bearer ${authInfo.value.authToken}`
        }

        return isAuthorized.value
    }

    const logout = () => {
        authInfo.value = {
            userId: 0,
            authToken: '',
            email: ''
        }
    }

    const userId = (): number => authInfo.value.userId

    return { isAuthorized, authorize, logout, userId }
})  