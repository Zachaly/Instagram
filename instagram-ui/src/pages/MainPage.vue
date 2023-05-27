<template>
    <AuthorizedPage>
        <div v-for="user of users" :key="user.id">
            {{ user.nickname }}
        </div>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import UserModel from '@/models/UserModel';
import AuthorizedPage from '@/pages/AuthorizedPage.vue';
import axios, { AxiosResponse } from 'axios';
import { Ref, onMounted, ref } from 'vue';

const users: Ref<UserModel[]> = ref([])

onMounted(() => {
    axios.get<any, AxiosResponse<UserModel[]>>('user').then(res => users.value = res.data)
})

</script>