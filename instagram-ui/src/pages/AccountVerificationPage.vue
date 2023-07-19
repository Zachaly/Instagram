<template>
    <AuthorizedPage :allowed-claims="[MODERATOR_CLAIM, ADMIN_CLAIM]">
        <NavigationPage>
            <div class="columns is-centered" v-if="verification">
                <div class="column is-3">
                    <p>First Name: {{ verification.firstName }}</p>
                    <p>Last Name: {{ verification.lastName }}</p>
                    <p>Date of birth: {{ verification.dateOfBirth }}</p>
                    <div class="buttons">
                        <button class="button is-info" @click="resolve(true)">Accept</button>
                        <button class="button is-warning" @click="resolve(false)">Deny</button>
                    </div>
                </div>
                <div class="column is-3">
                    <figure class="image is-3by4">
                        <img :src="image" alt="">
                    </figure>
                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { ADMIN_CLAIM, MODERATOR_CLAIM } from '@/constants';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import AccountVerificationModel from '@/models/AccountVerificationModel';
import { Ref, onMounted, ref } from 'vue';
import axios from 'axios';
import { useRoute, useRouter } from 'vue-router';
import ResolveAccountVerificationRequest from '@/models/request/ResolveAccountVerificationRequest';

const route = useRoute()
const router = useRouter()

const verification: Ref<AccountVerificationModel | null> = ref(null)
const image = ref('')

const resolve = (accepted: boolean) => {
    const request: ResolveAccountVerificationRequest = {
        id: verification.value!.id,
        accepted
    }

    axios.put('account-verification/resolve', request).then(res => {
        router.push('/moderation')
    })
}

onMounted(() => {
    const id = route.params['id'] as string
    axios.get<AccountVerificationModel>(`account-verification/${id}`).then(res => {
        verification.value = res.data
    }).catch(err => {
        router.push('/moderation')
    })
    axios.get(`image/account-verification/${id}`, {
        responseType: 'blob'
    }).then(res => {
        image.value = window.URL.createObjectURL(res.data)
    })
})

</script>