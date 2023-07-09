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
            <div class="is-flex">
                <div class="width-100">
                    <UserLinkComponent :nickName="post.creatorName" :id="post.creatorId" />
                </div>
                <div style="margin: auto;">
                    <div class="dropdown" :class="{'is-active': showDropdown}" >
                        <div class="dropdown-trigger">
                            <button  class="button" @click="showDropdown = !showDropdown">
                                More
                            </button>
                        </div>
                        <div class="dropdown-menu">
                            <div class="dropdown-content">
                                <a href="#" class="dropdown-item" @click="showReport = true">Report</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <p class="mt-3 mb-3 subtitle is-flex">
                Tags:
                <PostTagLink v-for="tag in post.tags" :key="tag" :tag="tag" />
            </p>
            <p>
                {{ post.content }}
            </p>
            <p class="subtitle is-4">
                <span @click="showLikes = post.likeCount > 0">
                    <font-awesome-icon :icon="['fas', 'heart']" />
                    {{ post.likeCount }}
                </span>
            </p>
            <p @click="showComments = !showComments">
                Comments({{ post.commentCount }})
            </p>
            <PostCommentList :post-id="post.id" v-if="showComments" />
        </div>
    </div>

    <div v-if="showLikes" class="fixed-center">
        <PostLikeListComponent :postId="post.id" @close="showLikes = false" />
    </div>

    <div v-if="showReport" class="fixed-center">
        <AddReportComponent :post-id="post.id" @close="showReport = false"/>
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
import PostCommentList from './PostCommentList.vue';
import PostTagLink from './PostTagLink.vue';
import AddReportComponent from './AddReportComponent.vue';

const currentImageIndex = ref(0)
const authStore = useAuthStore()
const showLikes = ref(false)
const showDropdown = ref(false)
const showReport = ref(false)
const props = defineProps<{
    post: PostModel
}>()

const changeIndex = (value: number) => {
    if (currentImageIndex.value + value < 0 || currentImageIndex.value + value > props.post.imageIds.length - 1) {
        return
    }
    currentImageIndex.value += value
}

const selectIndex = (index: number) => currentImageIndex.value = index

const showComments = ref(false)

const like = () => {
    if (!authStore.isAuthorized) {
        alert('You must be logged in to do that!')
        return
    }

    const getRequest: GetPostLikeRequest = { PostId: props.post.id, UserId: authStore.userId() }

    axios.get<number>('post-like/count', { params: getRequest }).then(res => {
        if (res.data > 0) {
            axios.delete(`post-like/${props.post.id}/${authStore.userId()}`).then()
        }
        else {
            const addRequest: AddPostLikeRequest = { postId: props.post.id, userId: authStore.userId() }
            axios.post('post-like', addRequest).then()
        }
    })
}
</script>