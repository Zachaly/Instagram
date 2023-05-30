<template>
    <NavigationPage>
        <div class="columns is-centered">
            <div class="column is-4">
                <UserCardComponent :user="user" />
                <p class="title">Posts</p>
            </div>
        </div>
        
    </NavigationPage>
</template>

<script setup lang="ts">
import NavigationPage from './NavigationPage.vue';
import UserCardComponent from '@/components/UserCardComponent.vue';
import UserModel from '@/models/UserModel';
import Gender from '@/models/enum/Gender';
import axios from 'axios';
import { onMounted, reactive } from 'vue';
import { useRoute } from 'vue-router'

const params = useRoute().params
const user: UserModel = reactive({
    name: '',
    nickname: '',
    bio: '',
    id: 0,
    gender: Gender.NotSpecified,
})

onMounted(() => {
    axios.get<UserModel>('/user/' + params.id).then(res => {
        Object.assign(user, res.data)
    })
})

</script>