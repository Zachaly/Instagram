<template>
    <div style="width: 30vw; max-width: 30%;" class="mb-4">
        <input type="text" class="input" v-model="searchVal" placeholder="search" @input="onSearch">
        <div style="position: fixed; z-index: 1;">
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

const users: Ref<UserModel[]> = ref([])

const searchVal = ref('')

const onSearch = () => {
    users.value = []
    if(searchVal.value.length < 2){
        return
    }

    const request: GetUserRequest = { SearchNickname: searchVal.value }

    axios.get<UserModel[]>('user', { params: request }).then(res => {
        users.value = res.data
    })
}
</script>