<template>
    <div class="box" style="min-width: 50vh; min-height: 30vh;">
        <div class="has-text-right">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <div class="control">
            <label for="reason">Reason</label>
            <input type="text" class="input" v-model="reason">
        </div>
        <div>
            <button class="button is-warning" @click="add">Add report</button>
        </div>
    </div>
</template>

<script setup lang="ts">
import AddPostReportRequest from '@/models/request/AddPostReportRequest';
import { useAuthStore } from '@/store/authStore';
import axios from 'axios';
import { ref } from 'vue';

const reason = ref('')
const props = defineProps<{
    postId: number
}>()
const authStore = useAuthStore()
const emit = defineEmits(['close'])

if(!authStore.isAuthorized){
    alert('You must be logged in to do that!')
    emit('close')
}

const add = () => {
    const request: AddPostReportRequest = {
        reportingUserId: authStore.userId(),
        reason: reason.value,
        postId: props.postId
    }

    axios.post('post-report', request).then(res => {
        alert('Report added!')
        emit('close')
    })
}

</script>