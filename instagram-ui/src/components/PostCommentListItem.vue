<template>
    <div class="box mr-4">
        <p class="subtitle is-6">
            {{ new Date(comment.created).toDateString() }}
        </p>
        <UserLinkComponent :nick-name="comment.userName" :id="comment.userId"/>
        <p>
            {{ comment.content }}
        </p>
    </div>
</template>

<script setup lang="ts">
import PostCommentModel from '@/models/PostCommentModel';
import UserLinkComponent from './UserLinkComponent.vue';

const props = defineProps<{
    comment: PostCommentModel
}>()
</script>