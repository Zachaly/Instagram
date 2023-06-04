<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns is-centered">
                <div class="column is-4">
                    <div class="control m-2">
                        <label class="label">Content</label>
                        <textarea rows="5" class="textarea" v-model="request.content">

                        </textarea>
                    </div>
                    <div class="control m-2">
                        <div class="file is-boxed">
                            <label class="file-label" style="width: 100%;">
                                <input type="file" @change="selectFile" class="file-input">
                                <span class="file-cta">
                                    <span class="file-icon">
                                        <i class="fas fa-upload"></i>
                                    </span>
                                    <span class="file-label">
                                        Select a new profile picture
                                    </span>
                                </span>
                            </label>
                        </div>
                    </div>
                    <div class="control m-2">
                        <button class="button is-info" @click="send">Add post</button>
                    </div>
                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import AddPostRequest from '@/models/request/AddPostRequest';
import ResponseModel from '@/models/ResponseModel';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import { reactive } from 'vue';
import { useAuthStore } from '@/store/authStore';
import axios, { AxiosError } from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter()

const request: AddPostRequest = reactive({
    creatorId: useAuthStore().userId(),
    content: '',
    file: undefined
})

const selectFile = (e: Event) => {
    request.file = (e.target! as HTMLInputElement).files![0]
}

const send = () => {
    if (!request.file) {
        alert("No file selected!")
    }

    const formData = new FormData()
    formData.append('File', request.file!)
    formData.append('CreatorId', request.creatorId.toString())
    formData.append('Content', request.content)

    axios.post('post', formData).then(() => {
        alert('Post added!')
        router.push('/')
    }).catch((err: AxiosError<ResponseModel>) => {
        console.log(err)
        alert(err.response?.data.error)
    })
}

</script>

<style scoped>
.column {
    margin-top: auto;
    margin-bottom: auto;
}

.columns {
    min-height: 80vh;
}
</style>