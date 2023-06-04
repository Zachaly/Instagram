<template>
    <div class="card m-1">
        <div class="card-image">
            <figure class="image is-4by3">
                <img :src="$image('post', post.id)">
            </figure>
        </div>
        <div class="card-content">
            <p class="title is-3">
                <router-link :to="{ name: 'user', params: { id: post.creatorId } }">
                    {{ post.creatorName }}
                </router-link>
            </p>

            {{ post.content }}
        </div>
    </div>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';

const props = defineProps<{
    post: PostModel
}>()


</script>