<template>
    <div class="columns is-centered">
        <div class="column is-6">
            <div v-for="ban in bans" :key="ban.id" class="card">
                <div class="card-content">
                    <UserLinkComponent :nick-name="ban.userName" :id="ban.userId" />
                    <div class="content">
                        {{ new Date(ban.startDate).toDateString() }} - {{ new Date(ban.endDate).toDateString() }}
                    </div>
                </div>
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