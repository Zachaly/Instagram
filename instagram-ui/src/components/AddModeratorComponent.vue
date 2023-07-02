<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <div class="control">
                <label class="label" for="name">Search user by name</label>
                <input type="text" class="input" @input="loadUsers" v-model="searchName">
            </div>
            <div>
                <div class="is-flex" v-for="user in users" :key="user.id">
                    <div style="width: 100%;">
                        <UserLinkComponent :nick-name="user.nickname" :id="user.id" />
                    </div>
                    <div style="margin: auto auto;">
                        <button class="button is-success" @click="addModerator(user.id)">Add moderator</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import UserModel from '@/models/UserModel';
import GetUserRequest from '@/models/request/get/GetUserRequest';
import axios from 'axios';
import { Ref, ref } from 'vue';
import UserLinkComponent from './UserLinkComponent.vue';
import GetUserClaimRequest from '@/models/request/get/GetUserClaimRequest';
import { MODERATOR_CLAIM } from '@/constants'
import AddUserClaimRequest from '@/models/request/AddUserClaimRequest';
import { useAuthStore } from '@/store/authStore';

const users: Ref<UserModel[]> = ref([])
const searchName = ref('')

const authStore = useAuthStore()

const loadUsers = () => {
    users.value = []
    if (searchName.value.length < 3) {
        return
    }

    const request: GetUserRequest = { SearchNickname: searchName.value, SkipIds: [authStore.userId()] }
    axios.get<UserModel[]>('user', { params: request }).then(res => users.value = res.data)
}

const addModerator = (id: number) => {
    const getRequest: GetUserClaimRequest = { UserId: id, Value: MODERATOR_CLAIM }

    axios.get<number>('user-claim/count', { params: getRequest }).then(res => {
        if (res.data > 0) {
            alert("User is already a moderator!")
            return
        }

        const addRequest: AddUserClaimRequest = {
            userId: id,
            value: MODERATOR_CLAIM
        }

        axios.post('user-claim', addRequest).then(res => {
            alert("Moderator added")
            users.value = users.value.filter(x => x.id !== id)
        })
    })
}

</script>