<template>
    <div class="container">
        <div class="columns">
            <div class="column col-4 col-mx-auto col-my-auto">
                <div>
                    <h2>Login</h2>
                    <div class="form-group">
                        <label class="form-label">Email</label>
                        <input class="form-input" v-model="request.email" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Password</label>
                        <input class="form-input" type="password" v-model="request.password" />
                    </div>
                    <button class="btn btn-success" @click="login">Login</button>
                    <div>
                        <h5>Do not have an account? <router-link to="/register" class="btn btn-link">Register</router-link>
                        </h5>
                    </div>
                </div>
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
        .then(res => {
            if (authStore.authorize(res.data)) {
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