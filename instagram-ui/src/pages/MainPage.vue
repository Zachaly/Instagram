<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns">
                <div class="column is-8">
                    <EndlessScrollComponent @on-bottom="onBottom" :style="'max-height: 90vh'">
                        <PostCardComponent v-for="post of posts" :post="post" :key="post.id" />
                    </EndlessScrollComponent>
                </div>
                <div class="column is-2">
                    <div v-for="user of users" :key="user.id">
                        <UserLinkComponent :nick-name="user.nickname" :id="user.id" />
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
import UserLinkComponent from '@/components/UserLinkComponent.vue';
import GetPostRequest from '@/models/request/get/GetPostRequest';
import { useAuthStore } from '@/store/authStore';
import GetUserRequest from '@/models/request/get/GetUserRequest';
import EndlessScrollComponent from '@/components/EndlessScrollComponent.vue';

const users: Ref<UserModel[]> = ref([])
const posts: Ref<PostModel[]> = ref([])
const authStore = useAuthStore()
const blockScroll = ref(false)

const PAGE_SIZE = 3;

const currentPageIndex = ref(0)

const loadPosts = () => {
    blockScroll.value = true
    if(authStore.userFollowsIds.length < 1){
        return
    }
    
    const request: GetPostRequest = { CreatorIds: [...authStore.userFollowsIds], PageIndex: currentPageIndex.value, PageSize: PAGE_SIZE  }
    axios.get<PostModel[]>('post', { params: request }).then(res => {
        posts.value.push(...res.data)
        currentPageIndex.value++
        blockScroll.value = res.data.length < PAGE_SIZE
    })
}

const loadUsers = () => {
    const request: GetUserRequest = { SkipIds: [...authStore.userFollowsIds, authStore.userId()] }
    axios.get<UserModel[]>('user', {
        params: request
    }).then(res => users.value = res.data)
}

const onBottom = () => {
    if(!blockScroll.value){
        loadPosts()
    }
}

onMounted(() => {
    loadUsers()
    loadPosts()
})
</script>