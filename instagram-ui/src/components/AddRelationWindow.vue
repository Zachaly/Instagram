<template>
    <div class="fixed-center box" style="min-width: 30vw;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <div>
            <div class="control">
                <label class="label">Name</label>
                <input class="input" type="text" v-model="request.name">
                <ValidationErrorListComponent v-if="validationErrors.Name" :errors="validationErrors.Name" />
            </div>
            <div class="control">
                <div class="file is-boxed">
                    <label class="file-label" style="width: 100%;">
                        <input type="file" multiple @change="selectFile" class="file-input">
                        <span class="file-cta">
                            <span class="file-icon">
                                <font-awesome-icon icon="fa-solid fa-image" />
                            </span>
                            <span class="file-label">
                                Select images
                            </span>
                        </span>
                    </label>
                </div>
                <ValidationErrorListComponent v-if="validationErrors.Files" :errors="validationErrors.Files"/>
            </div>
            <div>
                <button @click="add" class="button is-info">Add</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import AddRelationRequest from '@/models/request/AddRelationRequest';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { useAuthStore } from '@/store/authStore';
import { Ref, ref } from 'vue';
import axios, { AxiosError } from 'axios';
import ResponseModel from '@/models/ResponseModel';
import ValidationErrorListComponent from './ValidationErrorListComponent.vue';

const authStore = useAuthStore()

const request: Ref<AddRelationRequest> = ref({
    userId: authStore.userId(),
    name: ''
})

const validationErrors: Ref<{ Name?: string[], Files?: string[] }> = ref({})

const files: Ref<FileList | undefined> = ref(undefined)

const emit = defineEmits(["close"])

const selectFile = (e: Event) => {
    files.value = (e.target! as HTMLInputElement).files!
}

const add = () => {
    const data = new FormData()

    if (!files.value) {
        alert('Must select files!')
        return
    }

    data.append('UserId', request.value.userId.toString())
    data.append('Name', request.value.name)

    for (let i = 0; i < files.value.length; i++) {
        data.append('Files', files.value[i])
    }

    axios.post('relation', data).then(() => {
        alert('Relation added')
        emit('close')
    }).catch((err: AxiosError<ResponseModel>) => {
        validationErrors.value = err.response?.data.validationErrors
    })
}

</script>