<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns">
                <div class="column is-8">
                    <PostCardComponent v-for="post of posts" :post="post" :key="post.id" />
                </div>
                <div class="column is-2">
                    <div v-for="user of users" :key="user.id">
                        <router-link :to="{ name: 'user', params: { id: user.id } }">
                            {{ user.nickname }}
                        </router-link>
                    </div>
                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import UserModel from '@/models/UserModel';
import AuthorizedPage from '@/pages/AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import PostModel from '@/models/PostModel';
import PostCardComponent from '@/components/PostCardComponent.vue';

const users: Ref<UserModel[]> = ref([])
const posts: Ref<PostModel[]> = ref([])

const loadPosts = () => {
    axios.get<PostModel[]>('post').then(res => {
        posts.value = res.data
    })
}

onMounted(() => {
    axios.get<UserModel[]>('user').then(res => users.value = res.data)
    loadPosts()
})

</script>