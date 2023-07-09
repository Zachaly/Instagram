<template>
    <div>
        <div class="control" v-if="authStore.isAuthorized">
            <input class="input" v-model="addRequest.content"/>
            <button class="button is-info" @click="addComment">Add comment</button>
            <ValidationErrorListComponent v-if="validation.Content" :errors="validation.Content"/>
        </div>
        <PostCommentListItem :comment="comment" v-for="comment in comments" :key="comment.id" />
    </div>
</template>

<script setup lang="ts">
import PostCommentModel from '@/models/PostCommentModel';
import GetPostCommentRequest from '@/models/request/get/GetPostCommentRequest';
import axios, { AxiosError } from 'axios';
import { Ref, onMounted, ref } from 'vue';
import PostCommentListItem from './PostCommentListItem.vue';
import AddPostCommentRequest from '@/models/request/AddPostCommentRequest';
import { useAuthStore } from '@/store/authStore';
import ResponseModel from '@/models/ResponseModel';
import ValidationErrorListComponent from './ValidationErrorListComponent.vue';

const props = defineProps<{
    postId: number
}>()

const validation: Ref<{ Content?: string[]}> = ref({})

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
    }).catch((err: AxiosError<ResponseModel>) => {
        validation.value = err.response?.data.validationErrors
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