<template>
    <div class="container">
        <div class="columns">
            <div class="column col-4 col-mx-auto col-my-auto">
                <div>
                    <h2>Register</h2>
                    <div class="form-group">
                        <label class="form-label">Email</label>
                        <input class="form-input" v-model="request.email" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Name</label>
                        <input class="form-input" v-model="request.name" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Nickname</label>
                        <input class="form-input" v-model="request.nickname" />
                    </div>
                    <div>
                        <label class="form-label">Gender</label>
                        <select class="form-select" v-model="request.gender">
                            <option :value="Gender.NotSpecified" selected>Not specified</option>
                            <option :value="Gender.Man">Man</option>
                            <option :value="Gender.Woman">Woman</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Password</label>
                        <input class="form-input" type="password" v-model="request.password" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Confirm password</label>
                        <input class="form-input" type="password" v-model="confirmPassword" />
                    </div>
                    <button class="btn btn-success" @click="register">Register</button>
                    <div>
                        <h5>Have an account? <router-link to="/login" class="btn btn-link">Login</router-link>
                        </h5>
                    </div>
                </div>
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