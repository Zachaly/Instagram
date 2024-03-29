<template>
    <div class="card">
        <div class="card-content">
            <div class="media">
                <div class="media-left">
                    <figure class="image is-64x64">
                        <img @click="showStory = true && story !== null && authStore.isAuthorized"
                            :src="$image('profile', user.id)" alt="" class="is-rounded">
                    </figure>
                </div>
                <div class="media-content">
                    <p class="title">
                        {{ user.nickname }} <font-awesome-icon :icon="['fas', 'check']" v-if="user.verified" />
                        <button @click="follow"
                            v-if="!authStore.userFollowsIds.includes(user.id) && authStore.isAuthorized && user.id !== authStore.userId()"
                            class="button is-success">
                            Follow
                        </button>
                        <button @click="unFollow"
                            v-else-if="authStore.userFollowsIds.includes(user.id) && authStore.isAuthorized && user.id !== authStore.userId()"
                            class="button is-warning">
                            Unfollow
                        </button>
                        <button class="button is-danger" @click="blockUser"
                            v-if="authStore.isAuthorized && authStore.userId() !== user.id">
                            Block
                        </button>
                        <router-link class="button is-link" :to="{ name: 'chat', params: { userId: user.id } }"
                            v-if="authStore.isAuthorized && authStore.userId() !== user.id">
                            Chat
                        </router-link>
                    </p>
                    <p class="subtitle"> {{ user.name }}</p>
                </div>
                <div class="media-right" v-if="authStore.userId() == user.id">
                    <router-link to="/user/update">Update profile</router-link>
                </div>
            </div>
            <div>
                <span class="m-1">Posts: {{ postCount }}</span>
                <span class="m-1" @click="showFollows(true)">Followers: {{ followersCount }}</span>
                <span class="m-1" @click="showFollows(false)">Followed: {{ followedCount }}</span>
            </div>
            <div class="content">
                {{ user.bio }}
            </div>
        </div>
    </div>
    <div v-if="follows.length > 0" class="fixed-center">
        <FollowerListComponent :follows="follows" :follower="showFollowed" @close="closeFollows" />
    </div>

    <UserStoryWindowComponent v-if="story && showStory" :story="story" @close="showStory = false" />
</template>

<script setup lang="ts">
import UserFollowModel from '@/models/UserFollowModel';
import UserModel from '@/models/UserModel';
import AddUserFollowRequest from '@/models/request/AddUserFollowRequest';
import GetPostRequest from '@/models/request/get/GetPostRequest';
import GetUserFollowRequest from '@/models/request/get/GetUserFollowRequest';
import { useAuthStore } from '@/store/authStore';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import FollowerListComponent from './FollowerListComponent.vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import UserStoryModel from '@/models/UserStoryModel';
import GetUserStoryRequest from '@/models/request/get/GetUserStoryRequest';
import UserStoryWindowComponent from './UserStoryWindowComponent.vue';
import AddUserBlockRequest from '@/models/request/AddUserBlockRequest';
import router from '@/router';

const props = defineProps<{
    user: UserModel
}>()

const followedCount: Ref<number> = ref(0)
const followersCount: Ref<number> = ref(0)
const postCount: Ref<number> = ref(0)
const story: Ref<UserStoryModel | null> = ref(null)
const showStory = ref(false)

const authStore = useAuthStore()

const follow = () => {
    const request: AddUserFollowRequest = { followedUserId: props.user.id, followingUserId: authStore.userId() }
    axios.post('user-follow', request).then(() => {
        authStore.updateFollows()
    })
}

const unFollow = () => {
    axios.delete(`user-follow/${authStore.userId()}/${props.user.id}`).then(() => {
        authStore.updateFollows()
    })
}

const follows: Ref<UserFollowModel[]> = ref([])
const showFollowed: Ref<boolean> = ref(false)

const showFollows = (followed: boolean) => {
    showFollowed.value = followed
    if (!followed) {
        const request: GetUserFollowRequest = { FollowingUserId: props.user.id, JoinFollowed: true }
        axios.get<UserFollowModel[]>('user-follow', { params: request })
            .then(res => follows.value = res.data)
    } else {
        const request: GetUserFollowRequest = { FollowedUserId: props.user.id, JoinFollower: true }
        axios.get<UserFollowModel[]>('user-follow', { params: request })
            .then(res => follows.value = res.data)
    }
}

const closeFollows = () => {
    follows.value = []
}

const blockUser = () => {
    const request: AddUserBlockRequest = {
        blockingUserId: authStore.userId(),
        blockedUserId: props.user.id
    }

    axios.post('user-block', request).then(() => {
        alert('User blocked')
        router.push('/')
    })
}

onMounted(() => {
    const followRequest: GetUserFollowRequest = { FollowingUserId: props.user.id }
    axios.get<number>('user-follow/count', {
        params: followRequest
    }).then(res => followedCount.value = res.data)

    const followerRequest: GetUserFollowRequest = { FollowedUserId: props.user.id }
    axios.get<number>('user-follow/count', {
        params: followerRequest
    }).then(res => followersCount.value = res.data)

    const postRequest: GetPostRequest = { CreatorId: props.user.id }
    axios.get<number>('post/count', {
        params: postRequest
    }).then(res => postCount.value = res.data)

    const storyRequest: GetUserStoryRequest = { UserIds: [props.user.id] }
    axios.get<UserStoryModel[]>('user-story', { params: storyRequest }).then(res => {
        story.value = res.data[0]
    }).catch(() => { })
})
</script>