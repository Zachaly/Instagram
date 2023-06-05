<template>
    <div class="card">
        <div class="card-content">
            <div class="media">
                <div class="media-left">
                    <figure class="image is-64x64">
                        <img :src="$image('profile', user.id)" alt="" class="is-rounded">
                    </figure>
                </div>
                <div class="media-content">
                    <p class="title">{{ user.nickname }}</p>
                    <p class="subtitle"> {{ user.name }}</p>
                </div>
                <div class="media-right" v-if="authStore.userId() == user.id">
                    <router-link to="/user/update">Update profile</router-link>
                </div>
            </div>
            <div class="content">
                {{ user.bio }}
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import UserModel from '@/models/UserModel';
import { useAuthStore } from '@/store/authStore';

const props = defineProps<{
    user: UserModel
}>()

const authStore = useAuthStore()

</script>