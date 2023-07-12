<template>
    <div>
        <figure class="image is-64x64 m-1" @click="show">
            <img class="is-rounded" :src="$image('relation', relation.imageIds[0]!)" />
        </figure>
        <p class="subtitle has-text-centered"> {{ relation.name }}</p>
    </div>

    <RelationWindow :relation="relation" v-if="showWindow" @close="showWindow = false" @update="update"/>
    <UpdateRelationWindow :relation="relation" v-if="showUpdate" @close="showUpdate = false"/>
</template>

<script setup lang="ts">
import RelationModel from '@/models/RelationModel';
import { useAuthStore } from '@/store/authStore';
import { ref } from 'vue';
import RelationWindow from './RelationWindow.vue';
import UpdateRelationWindow from './UpdateRelationWindow.vue';

const authStore = useAuthStore()
const props = defineProps<{
    relation: RelationModel
}>()

const showWindow = ref(false)
const showUpdate = ref(false)

const show = () => {
    if(!authStore.isAuthorized){
        alert('You must be logged to do that')
        return
    }
    showWindow.value = true
}

const update = () => {
    showWindow.value = false
    showUpdate.value = true
}

</script>