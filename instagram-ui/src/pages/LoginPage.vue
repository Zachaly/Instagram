<template>
    <div class="columns is-centered">
        <div class="column is-4">
            <h2 class="title">Login</h2>
            <div class="control">
                <label class="label">Email</label>
                <input class="input" v-model="request.email" />
            </div>
            <div class="control">
                <label class="label">Password</label>
                <input class="input" type="password" v-model="request.password" />
            </div>
            <button class="button is-success" @click="login">Login</button>
            <div class="control">
                <h5>Do not have an account?</h5>
                <router-link to="/register" class="button is-link">Register</router-link>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import LoginRequest from '@/models/request/LoginRequest';
import { reactive, toRaw } from 'vue';
import axios, { AxiosError, AxiosResponse } from 'axios';
import LoginResponse from '@/models/LoginResponse';
import ResponseModel from '@/models/ResponseModel';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/store/authStore'

const request: LoginRequest = reactive({
    email: '',
    password: ''
})

const router = useRouter()
const authStore = useAuthStore()

const login = () => {
    axios.post<LoginRequest, AxiosResponse<LoginResponse>>('user/login', toRaw(request))
        .then(async (res) => {
            if (await authStore.authorize(res.data)) {
                router.push('/')
            }
        })
        .catch((err: AxiosError<ResponseModel>) => alert(err.response?.data.error))
}
</script>

<style scoped>
.columns {
    min-height: 80vh;
}

.columns>div {
    margin-top: auto;
    margin-bottom: auto;
}
</style>