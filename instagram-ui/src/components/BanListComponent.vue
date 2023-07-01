<template>
    <div v-for="ban in bans" :key="ban.id" class="card">
        <div class="card-content">
            <div class="media">
                <UserLinkComponent :nick-name="ban.userName" :id="ban.userId"/>
            </div>
            <div class="content">
                {{ new Date(ban.startDate).toDateString() }} - {{ new Date(ban.endDate).toDateString() }}
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import UserBanModel from '@/models/UserBanModel';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import UserLinkComponent from './UserLinkComponent.vue';

const bans: Ref<UserBanModel[]> = ref([])

onMounted(() => {
    axios.get<UserBanModel[]>('user-ban').then(res => bans.value = res.data)
})

</script>