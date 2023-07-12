<template>
    <div class="is-flex">
        <RelationListItemComponent v-for="relation in relations" :relation="relation" :key="relation.id"/>
        <div v-if="props.userId == authStore.userId()">
            <button class="button" @click="addingRelation = true">Add new</button>
        </div>
    </div>
    <AddRelationWindow v-if="addingRelation" @close="addingRelation = false"/>
</template>

<script setup lang="ts">
import RelationModel from '@/models/RelationModel';
import GetRelationRequest from '@/models/request/get/GetRelationRequest';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import RelationListItemComponent from './RelationListItemComponent.vue';
import { useAuthStore } from '@/store/authStore';
import AddRelationWindow from './AddRelationWindow.vue';

const props = defineProps<{
    userId: number
}>()

const authStore = useAuthStore()

const relations: Ref<RelationModel[]> = ref([])

const addingRelation = ref(false)

onMounted(() => {
    const params: GetRelationRequest = {
        UserId: props.userId
    }

    axios.get<RelationModel[]>('relation', { params }).then(res => {
        relations.value = res.data
    })
})
</script>