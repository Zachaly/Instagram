<template>
    <div class="box">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <UserLinkComponent v-for="like in likes" :key="like.userId" :id="like.userId" :nick-name="like.userName" />
    </div>
</template>

<script setup lang="ts">
import PostLikeModel from '@/models/PostLikeModel';
import GetPostLikeRequest from '@/models/request/get/GetPostLikeRequest';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import UserLinkComponent from './UserLinkComponent.vue';

const likes: Ref<PostLikeModel[]> = ref([])

const props = defineProps<{
    postId: number
}>()

defineEmits(['close'])

onMounted(() => {
    const getRequest: GetPostLikeRequest = { PostId: props.postId }

    axios.get<PostLikeModel[]>('post-like', { params: getRequest }).then(res => {
        likes.value = res.data
    })
})
</script>