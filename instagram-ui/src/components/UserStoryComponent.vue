<template>
    <div>
        <figure class="image is-64x64 m-1" @click="showWindow = true">
            <img class="is-rounded" :src="$image('story-image', story.images[0]!.id)" />
        </figure>
        <p class="subtitle has-text-centered"> {{ story.userName }}</p>

        <UserStoryWindowComponent :story="story" v-if="showWindow" @close="showWindow = false"/>
    </div>
</template>

<script setup lang="ts">
import UserStoryModel from '@/models/UserStoryModel';
import { ref } from 'vue';
import UserStoryWindowComponent from './UserStoryWindowComponent.vue';

const showWindow = ref(false)

const props = defineProps<{
    story: UserStoryModel
}>()

</script>