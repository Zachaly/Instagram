import LoginResponse from "@/models/LoginResponse";
import UserFollowModel from "@/models/UserFollowModel";
import GetUserFollowRequest from "@/models/request/GetUserFollowRequest";
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

    const userFollowsIds: Ref<number[]> = ref([])

    const authorize = (response: LoginResponse) => {
        if (!response.authToken) {
            isAuthorized.value = false
        } else {
            authInfo.value = response
            isAuthorized.value = true
            axios.defaults.headers.common.Authorization = `Bearer ${authInfo.value.authToken}`
            updateFollows()
        }

        return isAuthorized.value
    }

    const updateFollows = () => {
        const followRequest: GetUserFollowRequest = { FollowingUserId: authInfo.value.userId, SkipPagination: true }
        axios.get<UserFollowModel[]>('user-follow', {
            params: followRequest
        }).then(res => userFollowsIds.value = res.data.map(x => x.followedUserId))
    }

    const logout = () => {
        authInfo.value = {
            userId: 0,
            authToken: '',
            email: ''
        }
    }

    const userId = (): number => authInfo.value.userId

    return { isAuthorized, authorize, logout, userId, updateFollows, userFollowsIds }
})  