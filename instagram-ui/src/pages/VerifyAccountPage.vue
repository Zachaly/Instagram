<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns is-centered">
                <div class="column is-5">
                    <div class="control">
                        <label class="label">First name</label>
                        <input type="text" class="input" v-model="request.firstName">
                        <ValidationErrorListComponent v-if="validationErrors.FirstName" :errors="validationErrors.FirstName"/>
                    </div>
                    <div class="control">
                        <label class="label">Last name</label>
                        <input type="text" class="input" v-model="request.lastName">
                        <ValidationErrorListComponent v-if="validationErrors.LastName" :errors="validationErrors.LastName"/>
                    </div>
                    <div class="control">
                        <label class="label">Date of birth</label>
                        <input type="text" class="input" v-model="request.dateOfBirth">
                        <ValidationErrorListComponent v-if="validationErrors.DateOfBirth" :errors="validationErrors.DateOfBirth"/>
                    </div>
                    <div class="control">
                        <label class="label">Verification document</label>
                        <input type="file" class="input" @change="selectFile">
                    </div>
                    <div class="control">
                        <button class="button is-info" @click="send">Send</button>
                    </div>
                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { Ref, ref } from 'vue';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import AddAccountVerificationRequest from '@/models/request/AddAccountVerificationRequest';
import { useAuthStore } from '@/store/authStore';
import ValidationErrorListComponent from '@/components/ValidationErrorListComponent.vue';
import axios, { AxiosError } from 'axios';
import { useRouter } from 'vue-router';
import ResponseModel from '@/models/ResponseModel';

const authStore = useAuthStore()
const router = useRouter()

const request: Ref<AddAccountVerificationRequest> = ref({
    userId: authStore.userId(),
    firstName: '',
    lastName: '',
    dateOfBirth: ''
})

const validationErrors: Ref<{
    UserId?: string[],
    FirstName?: string[],
    LastName?: string[],
    DateOfBirth?: string[]
}> = ref({})

const verificationDocument: Ref<File | null> = ref(null)

const selectFile = (e: Event) => {
    const target = e.target as HTMLInputElement

    verificationDocument.value = target.files![0]
}

const send = () => {
    const formData = new FormData()

    if(!verificationDocument.value){
        alert('Must select file!')
        return
    }

    formData.append('UserId', request.value.userId.toString())
    formData.append('FirstName', request.value.firstName)
    formData.append('LastName', request.value.lastName)
    formData.append('DateOfBirth', request.value.dateOfBirth)
    formData.append('Document', verificationDocument.value)

    axios.post('account-verification', formData).then(() => {
        router.push('/')
    }).catch((err: AxiosError<ResponseModel>) => {
        console.log(err)
        validationErrors.value = err.response?.data.validationErrors
    })
}

</script>