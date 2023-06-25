<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <div class="is-flex" v-for="user in moderators" :key="user.userId">
                <div style="width: 100%;">
                    <UserLinkComponent :nick-name="user.userName" :id="user.userId"/>
                </div>
                <div style="margin: auto;">
                    <button class="button is-danger" @click="deleteModerator(user.userId)">Delete</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { MODERATOR_CLAIM } from '@/constants';
import UserClaimModel from '@/models/UserClaimModel';
import GetUserClaimRequest from '@/models/request/get/GetUserClaimRequest';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import UserLinkComponent from './UserLinkComponent.vue';

const moderators: Ref<UserClaimModel[]> = ref([])

const loadModerators = () => {
    const params: GetUserClaimRequest = { Value: MODERATOR_CLAIM }

    axios.get<UserClaimModel[]>('user-claim', { params }).then(res => {
        moderators.value = res.data
    })
}

const deleteModerator = (id: number) => {
    axios.delete(`user-claim/${id}/${MODERATOR_CLAIM}`).then(res => {
        alert("Moderator deleted!")
        loadModerators()
    })
}

onMounted(() => {
    loadModerators()
})



</script>