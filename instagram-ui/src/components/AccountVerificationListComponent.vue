<template>
    <EndlessScrollComponent @on-bottom="onBottom" :style="'max-height: 40vh;'">
        <div class="box" v-for="verification in verifications" :key="verification.id">
            <router-link :to="{ name: 'account-verification', params: { id: verification.id }}">
                {{ verification.firstName }} {{ verification.lastName }} - {{ verification.userName }}
            </router-link>
        </div>
    </EndlessScrollComponent>
</template>

<script setup lang="ts">
import { Ref, onMounted, ref } from 'vue';
import EndlessScrollComponent from './EndlessScrollComponent.vue';
import axios from 'axios';
import AccountVerificationModel from '@/models/AccountVerificationModel';
import GetAccountVerificationRequest from '@/models/request/get/GetAccountVerificationRequest';

const pageIndex = ref(0)
const blockScroll = ref(false)
const verifications: Ref<AccountVerificationModel[]> = ref([])

const loadVerifications = () => {
    blockScroll.value = true

    const params: GetAccountVerificationRequest = {
        PageIndex: pageIndex.value
    }
    axios.get<AccountVerificationModel[]>('account-verification', { params }).then(res => {
        verifications.value.push(...res.data)
        pageIndex.value++
        blockScroll.value = res.data.length > 0
    })
}

const onBottom = () => {
    if (!blockScroll.value) {
        loadVerifications()
    }
}

onMounted(() => {
    loadVerifications()
})
</script>