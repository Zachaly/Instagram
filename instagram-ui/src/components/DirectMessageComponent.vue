<template>
    <div class="box is-flex">
        <div class="mr-2">
            <figure class="image is-64x64">
                <img :src="$image('profile', message.senderId)" class="rounded" alt="">
            </figure>
        </div>
        <div>
            <p class="subtitle mb-1">{{ new Date(message.created).toDateString() }}</p>
            <p>{{ message.content }}</p>
            <div v-if="message.read" style="background-color: rgb(58, 221, 58); height: 1vh; width: 100%;">

            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import DirectMessageModel from '@/models/DirectMessageModel';

defineProps<{
    message: DirectMessageModel
}>()
</script>