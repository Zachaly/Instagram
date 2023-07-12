<template>
    <div class="fixed-center box" style="width: 40vh;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <div class="has-text-centered">
            <button class="button is-danger" @click="remove">Remove relation</button>
        </div>
        <div class="is-flex" style="overflow-x: scroll;">
            <div v-for="id in imageIds" :key="id" class="m-1">
                <figure class="image image is-3by5">
                    <img :src="$image('relation', id)">
                </figure>
                <button class="button is-warning" @click="removeImage(id)">Remove</button>
            </div>
        </div>
        <div>
            <div class="control">
                <label class="label">Add new image</label>
                <input type="file" class="input" @change="selectFile">
            </div>
            <div class="control">
                <button @click="addImage" class="button is-info">Add image</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import RelationModel from '@/models/RelationModel';
import axios from 'axios';
import { Ref, ref } from 'vue';

const props = defineProps<{
    relation: RelationModel
}>()

const imageIds = ref(props.relation.imageIds)

const emit = defineEmits(['close'])

const file: Ref<File | undefined> = ref(undefined)

const remove = () => {
    axios.delete(`relation/${props.relation.id}`).then(() => {
        emit('close')
    })
}

const removeImage = (id: number) => {
    axios.delete(`relation-image/${id}`).then(() => {
        imageIds.value = imageIds.value.filter(x => x !== id)
    })
}

const selectFile = (e: Event) => {
    file.value = (e.target as HTMLInputElement).files?.item(0) ?? undefined
}

const addImage = () => {
    if (!file.value) {
        alert('Select file!')
        return
    }

    const data = new FormData()

    data.append('RelationId', props.relation.id.toString())
    data.append('File', file.value)

    axios.post('relation-image', data).then(() => {
        file.value = undefined
        axios.get<RelationModel>(`relation/${props.relation.id}`).then(res => {
            imageIds.value = res.data.imageIds
        })
    })
}

</script>