<template>
    <div class="columns is-centered">
        <div class="column is-4 ">
            <h2 class="title">Register</h2>
            <div class="control">
                <label class="label">Email</label>
                <input class="input" v-model="request.email" />
            </div>
            <div class="control">
                <label class="label">Name</label>
                <input class="input" v-model="request.name" />
            </div>
            <div class="control">
                <label class="label">Nickname</label>
                <input class="input" v-model="request.nickname" />
            </div>
            <div>
                <label class="label">Gender</label>
                <div class="select">
                    <select class="select" v-model="request.gender">
                        <option :value="Gender.NotSpecified" selected>Not specified</option>
                        <option :value="Gender.Man">Man</option>
                        <option :value="Gender.Woman">Woman</option>
                    </select>
                </div>

            </div>
            <div class="control">
                <label class="label">Password</label>
                <input class="input" type="password" v-model="request.password" />
            </div>
            <div class="control">
                <label class="label">Confirm password</label>
                <input class="input" type="password" v-model="confirmPassword" />
            </div>
            <button class="button is-success" @click="register">Register</button>
            <div class="control">
                <h5>Have an account?</h5>
                <router-link to="/login" class="button is-link">Login</router-link>
            </div>
        </div>
    </div>
</template>

<style scoped>
.columns {
    min-height: 80vh;
}

.columns>div {
    margin-top: auto;
    margin-bottom: auto;
}
</style>

<script setup lang="ts">
import Gender from '@/models/enum/Gender';
import RegisterRequest from '@/models/request/RegisterRequest';
import { reactive, ref, toRaw } from 'vue';
import axios, { AxiosError } from 'axios'
import { useRouter } from 'vue-router';
import ResponseModel from '@/models/ResponseModel'


const request: RegisterRequest = reactive({
    name: '',
    nickname: '',
    email: '',
    password: '',
    gender: Gender.NotSpecified,
})

const confirmPassword = ref('')

const router = useRouter()

const register = () => {
    if (confirmPassword.value !== request.password) {
        alert('Passwords do not match!')
        return
    }
    const body = toRaw(request)

    axios.post<RegisterRequest>('user', body).then(() => {
        alert('Account created!')
        router.push('/login')
    }).catch((err: AxiosError<any, ResponseModel>) => alert(err.response!.data.error!))
}

</script>