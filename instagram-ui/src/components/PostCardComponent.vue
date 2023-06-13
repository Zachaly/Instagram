<template>
    <div class="card m-1">
        <div @dblclick="like">
            <div class="card-image">
                <figure class="image is-4by3">
                    <img :src="$image('post', post.imageIds[currentImageIndex])">
                </figure>
            </div>
        </div>

        <div class="is-flex pr-2 pl-2 mt-1">
            <font-awesome-icon @click="changeIndex(-1)" size="xl" :icon="['fass', 'caret-left']" />
            <div class="is-flex is-justify-content-center" style="width: 100%;">
                <font-awesome-icon class="m-1" v-for="(id, index) of post.imageIds" :key="id"
                    :icon="['fas', index == currentImageIndex ? 'circle' : 'circle-notch']" size="xl"
                    @click="selectIndex(index)" />
            </div>
            <font-awesome-icon @click="changeIndex(1)" size="xl" :icon="['fass', 'caret-right']" />
        </div>

        <div class="card-content">
            <UserLinkComponent :nickName="post.creatorName" :id="post.creatorId" />
            <p>
                {{ post.content }}
            </p>
            <p class="subtitle is-4">
                <span @click="showLikes = post.likeCount > 0">
                    <font-awesome-icon :icon="['fas', 'heart']" />
                    {{ post.likeCount }}
                </span>
            </p>
        </div>
    </div>

    <div v-if="showLikes" class="fixed-center">
        <PostLikeListComponent :postId="post.id" @close="showLikes = false" />
    </div>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';
import UserLinkComponent from './UserLinkComponent.vue';
import { ref } from 'vue';
import { useAuthStore } from '@/store/authStore';
import GetPostLikeRequest from '@/models/request/get/GetPostLikeRequest';
import axios from 'axios';
import AddPostLikeRequest from '@/models/request/AddPostLikeRequest';
import PostLikeListComponent from './PostLikeListComponent.vue';

const currentImageIndex = ref(0)
const authStore = useAuthStore()
const showLikes = ref(false)

const changeIndex = (value: number) => {
    if (currentImageIndex.value + value < 0 || currentImageIndex.value + value > props.post.imageIds.length - 1) {
        return
    }
    currentImageIndex.value += value
}

const selectIndex = (index: number) => currentImageIndex.value = index

const props = defineProps<{
    post: PostModel
}>()

const like = () => {
    if (!authStore.isAuthorized) {
        alert('You must be logged in to do that!')
        return
    }

    const getRequest: GetPostLikeRequest = { PostId: props.post.id, UserId: authStore.userId() }

    axios.get<number>('post-like/count', { params: getRequest }).then(res => {
        if (res.data > 0) {
            console.log('del')
            axios.delete(`post-like/${props.post.id}/${authStore.userId()}`).then()
        }
        else {
            console.log('add')
            const addRequest: AddPostLikeRequest = { postId: props.post.id, userId: authStore.userId() }
            axios.post('post-like', addRequest).then()
        }
    })
}

</script>