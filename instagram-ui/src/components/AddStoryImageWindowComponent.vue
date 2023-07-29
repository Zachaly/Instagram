<template>
    <div class="fixed-center box" style="min-width: 30vw;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <div>
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
            </div>
            <div>
                <button @click="add" class="button is-info">Add</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { useAuthStore } from '@/store/authStore';
import { Ref, ref } from 'vue';
import axios from 'axios';

const authStore = useAuthStore()
const files: Ref<FileList | null> = ref(null)

const emit = defineEmits(['close'])

const add = () => {
    const data = new FormData()

    if(!files.value){
        alert('Must select images!')
        return
    }

    data.append('UserId', authStore.userId().toString())
    for(let i = 0; i < files.value.length; i++){
        data.append('Images', files.value[i])
    }

    axios.post('user-story', data).then(() => {
        emit('close')
    })
}

const selectFile = (e: Event) => {
    const target = e.target as HTMLInputElement

    files.value = target.files
}
</script>