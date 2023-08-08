<template>
    <div style="width: 30vw; max-width: 30%;" class="mb-4">
        <input type="text" class="input" v-model="searchVal" placeholder="search" @input="onSearch">
        <div v-if="users.length > 0" style="position: fixed; z-index: 1; width: 25vw; max-width: 30%;" class="has-background-light">
            <UserLinkComponent v-for="user in users" :key="user.id" :id="user.id" :nick-name="user.nickname"/>
        </div>
    </div>
</template>

<script lang="ts" setup>
import UserModel from '@/models/UserModel';
import GetUserRequest from '@/models/request/get/GetUserRequest';
import axios from 'axios';
import { Ref, ref } from 'vue';
import UserLinkComponent from './UserLinkComponent.vue';
import { useAuthStore } from '@/store/authStore';

const users: Ref<UserModel[]> = ref([])
const authStore = useAuthStore()

const searchVal = ref('')

const onSearch = () => {
    users.value = []
    if(searchVal.value.length < 2){
        return
    }

    const request: GetUserRequest = { SearchNickname: searchVal.value, SkipIds: [...authStore.blockerIds] }

    axios.get<UserModel[]>('user', { params: request }).then(res => {
        users.value = res.data
    })
}
</script>