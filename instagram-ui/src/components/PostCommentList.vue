<template>
    <div>
        <div class="control" v-if="authStore.isAuthorized">
            <input class="input" v-model="addRequest.content"/>
            <button class="button is-info" @click="addComment">Add comment</button>
        </div>
        <PostCommentListItem :comment="comment" v-for="comment in comments" :key="comment.id" />
    </div>
</template>

<script setup lang="ts">
import PostCommentModel from '@/models/PostCommentModel';
import GetPostCommentRequest from '@/models/request/get/GetPostCommentRequest';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import PostCommentListItem from './PostCommentListItem.vue';
import AddPostCommentRequest from '@/models/request/AddPostCommentRequest';
import { useAuthStore } from '@/store/authStore';

const props = defineProps<{
    postId: number
}>()

const authStore = useAuthStore()

const comments: Ref<PostCommentModel[]> = ref([])

const addRequest: Ref<AddPostCommentRequest> = ref({
    postId: props.postId,
    userId: authStore.userId(),
    content: ''
})

const addComment = () => {
    axios.post('post-comment', addRequest.value).then(res => {
        addRequest.value.content = ''
        loadComments()
    })
}

const loadComments = () => {
    const params: GetPostCommentRequest = { PostId: props.postId }
    axios.get<PostCommentModel[]>('post-comment', { params }).then(res => {
        comments.value = res.data
    })
}

onMounted(() => {
    loadComments()
})
</script>