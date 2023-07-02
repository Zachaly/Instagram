<template>
    <router-link class="m-1" :to="{ name: 'post', params: { id: post.id } }">
        <figure class="image is-128x128" style="width:;">
            <img :src="$image('post', post.imageIds[0])">
        </figure>
    </router-link>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';

const props = defineProps<{
    post: PostModel
}>()
</script>