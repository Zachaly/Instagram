<template>
    <div class="card m-1">
        <div class="card-image">
            <figure class="image is-4by3">
                <img :src="$image('post', post.id)">
            </figure>
        </div>
        <div class="card-content">
            <UserLinkComponent :nickName="post.creatorName" :id="post.creatorId"/>

            <p>
                {{ post.content }}
            </p>
        </div>
    </div>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';
import UserLinkComponent from './UserLinkComponent.vue';

const props = defineProps<{
    post: PostModel
}>()


</script>