<template>
    <div class="card m-1">
        <div>
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
            <UserLinkComponent :nickName="post.creatorName" :id="post.creatorId" />

            <p>
                {{ post.content }}
            </p>
        </div>
    </div>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';
import UserLinkComponent from './UserLinkComponent.vue';
import { ref } from 'vue';

const currentImageIndex = ref(0)

const changeIndex = (value: number) => {
    if(currentImageIndex.value + value < 0 || currentImageIndex.value + value > props.post.imageIds.length - 1){
        return
    }
    currentImageIndex.value += value
}

const selectIndex = (index: number) => currentImageIndex.value = index

const props = defineProps<{
    post: PostModel
}>()


</script>