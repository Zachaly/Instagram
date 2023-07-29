<template>
    <div class="fixed-center box" style="min-width: 20vw; top: 5vh;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <p class="title has-text-centered">
            {{ story.userName }}
        </p>
        <div>
            <p class="subtitle">{{ new Date(story.images[currentImageIndex].created).toString() }}</p>
            <figure class="image is-3by4" >
                <img :src="$image('story-image', story.images[currentImageIndex].id)" alt="">
            </figure>
        </div>
        <div class="is-flex">
            <font-awesome-icon @click="changeIndex(-1)" size="xl" :icon="['fass', 'caret-left']" />
            <div class="is-flex is-justify-content-center" style="width: 100%;">
                <font-awesome-icon class="m-1" v-for="(image, index) of story.images" :key="image.id"
                    :icon="['fas', index == currentImageIndex ? 'circle' : 'circle-notch']" size="xl"
                    @click="selectIndex(index)" />
            </div>
            <font-awesome-icon @click="changeIndex(1)" size="xl" :icon="['fass', 'caret-right']" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import UserStoryModel from '@/models/UserStoryModel';
import { ref } from 'vue';

const props = defineProps<{
    story: UserStoryModel
}>()

const currentImageIndex = ref(0)

const emit = defineEmits(['close', 'update'])

const selectIndex = (index: number) => {
    currentImageIndex.value = index
}

const changeIndex = (value: number) => {
    if (currentImageIndex.value + value < 0) {
        return
    }

    if (currentImageIndex.value + value > props.story.images.length - 1) {
        emit('close')
        return
    }

    currentImageIndex.value += value
}

</script>