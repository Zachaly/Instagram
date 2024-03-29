<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns">
                <div class="column is-8">
                    <div class="box is-flex">
                        <button class="button" @click="showAddStoryWindow = true">Add story</button>
                        <UserStoryComponent v-for="story in stories" :key="story.userId" :story="story" />
                    </div>
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

            <AddStoryImageWindowComponent v-if="showAddStoryWindow" @close="showAddStoryWindow = false" />
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
import UserStoryModel from '@/models/UserStoryModel';
import GetUserStoryRequest from '@/models/request/get/GetUserStoryRequest';
import AddStoryImageWindowComponent from '@/components/AddStoryImageWindowComponent.vue';
import UserStoryComponent from '@/components/UserStoryComponent.vue';

const users: Ref<UserModel[]> = ref([])
const posts: Ref<PostModel[]> = ref([])
const stories: Ref<UserStoryModel[]> = ref([])
const authStore = useAuthStore()
const blockScroll = ref(false)
const showAddStoryWindow = ref(false)

const PAGE_SIZE = 3;

const currentPageIndex = ref(0)

const loadPosts = () => {
    blockScroll.value = true
    if (authStore.userFollowsIds.length < 1) {
        return
    }

    const request: GetPostRequest = {
        CreatorIds: [...authStore.userFollowsIds],
        SkipCreators: [...authStore.blockerIds ],
        PageIndex: currentPageIndex.value,
        PageSize: PAGE_SIZE
    }
    axios.get<PostModel[]>('post', { params: request }).then(res => {
        posts.value.push(...res.data)
        currentPageIndex.value++
        blockScroll.value = res.data.length < PAGE_SIZE
    })
}

const loadUsers = () => {
    const request: GetUserRequest = { SkipIds: [...authStore.userFollowsIds, authStore.userId(), ...authStore.blockerIds] }
    axios.get<UserModel[]>('user', {
        params: request
    }).then(res => users.value = res.data)
}

const loadStories = () => {
    const params: GetUserStoryRequest = { UserIds: [...authStore.userFollowsIds] }

    axios.get<UserStoryModel[]>('user-story', { params }).then(res => {
        stories.value = res.data
        console.log(res.data)
    }).catch(err => console.log(err))
}

const onBottom = () => {
    if (!blockScroll.value) {
        loadPosts()
    }
}

onMounted(() => {
    loadUsers()
    loadPosts()
    loadStories()
})
</script>