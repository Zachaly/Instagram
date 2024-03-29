import LoginResponse from "@/models/LoginResponse";
import UserBlockModel from "@/models/UserBlockModel";
import UserFollowModel from "@/models/UserFollowModel";
import GetUserBlockRequest from "@/models/request/get/GetUserBlockRequest";
import GetUserFollowRequest from "@/models/request/get/GetUserFollowRequest";
import axios from "axios";
import { defineStore } from "pinia";
import { Ref, ref } from 'vue'

const TOKEN_ITEM = 'token'

export const useAuthStore = defineStore('auth', () => {
    const isAuthorized = ref(false)
    const authInfo: Ref<LoginResponse> = ref({
        userId: 0,
        authToken: '',
        email: '',
        claims: []
    })

    const userFollowsIds: Ref<number[]> = ref([])
    const blockerIds: Ref<number[]> = ref([])

    const authorize = async (response: LoginResponse, remember: boolean) => {
        if (!response.authToken) {
            isAuthorized.value = false
        } else {
            authInfo.value = response
            isAuthorized.value = true
            axios.defaults.headers.common.Authorization = `Bearer ${authInfo.value.authToken}`
            if (remember) {
                localStorage.setItem(TOKEN_ITEM, response.authToken)
            }
            await updateFollows()
            await updateBlockers()
        }

        return isAuthorized.value
    }

    const updateFollows = (): Promise<any> => new Promise((resolve, reject) => {
        const followRequest: GetUserFollowRequest = { FollowingUserId: authInfo.value.userId, SkipPagination: true }
        axios.get<UserFollowModel[]>('user-follow', {
            params: followRequest
        }).then(res => {
            userFollowsIds.value = res.data.map(x => x.followedUserId)
            resolve(true)
        })
    })

    const updateBlockers = (): Promise<any> => new Promise((resolve, reject) => {
        const blockRequest: GetUserBlockRequest = { BlockedUserId: authInfo.value.userId, SkipPagination: true }

        axios.get<UserBlockModel[]>('user-block', {
            params: blockRequest
        }).then(res => {
            blockerIds.value = res.data.map(x => x.blockingUserId)
            resolve(null)
        })
    })

    const logout = () => {
        authInfo.value = {
            userId: 0,
            authToken: '',
            email: '',
            claims: []
        }
        localStorage.setItem(TOKEN_ITEM, '')
    }

    const hasClaim = (claim: string): boolean => authInfo.value.claims.includes(claim)

    const userId = (): number => authInfo.value.userId

    const token = () => authInfo.value.authToken

    const loadFromSavedToken = (): Promise<boolean> => new Promise((resolve, reject) => {
        const token = localStorage.getItem(TOKEN_ITEM)
        if (!token) {
            resolve(false)
            return
        }
        axios.get<LoginResponse>('user/current', {
            headers: {
                Authorization: 'Bearer ' + token
            }
        }).then(async (res) => {
            resolve(await authorize(res.data, true))
        })
    })

    return { isAuthorized, authorize, logout, userId, updateFollows, userFollowsIds, hasClaim, loadFromSavedToken, token, blockerIds }
})  