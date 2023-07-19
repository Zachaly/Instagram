<template>
    <div class="fixed-center box" style="min-width: 20vw;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <div>
            <button v-if="relation.userId == authStore.userId()" class="button is-info" @click="$emit('update')">Update</button>
        </div>
        <p class="title has-text-centered">
            {{ relation.userName }}
        </p>
        <div>
            <figure class="image is-3by5">
                <img :src="$image('relation', relation.imageIds[currentImageIndex])" alt="">
            </figure>
        </div>
        <div class="is-flex">
            <font-awesome-icon @click="changeIndex(-1)" size="xl" :icon="['fass', 'caret-left']" />
            <div class="is-flex is-justify-content-center" style="width: 100%;">
                <font-awesome-icon class="m-1" v-for="(id, index) of relation.imageIds" :key="id"
                    :icon="['fas', index == currentImageIndex ? 'circle' : 'circle-notch']" size="xl"
                    @click="selectIndex(index)" />
            </div>
            <font-awesome-icon @click="changeIndex(1)" size="xl" :icon="['fass', 'caret-right']" />
        </div>
    </div>
</template>

<script setup lang="ts">
import RelationModel from '@/models/RelationModel';
import { useAuthStore } from '@/store/authStore';
import { ref } from 'vue';

const authStore = useAuthStore()
const props = defineProps<{
    relation: RelationModel
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

    if (currentImageIndex.value + value > props.relation.imageIds.length - 1) {
        emit('close')
        return
    }

    currentImageIndex.value += value
}

</script>
