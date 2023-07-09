<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns is-centered">
                <div class="column is-6">
                    <div class="control m-2">
                        <label class="label">Content</label>
                        <textarea rows="2" class="textarea" v-model="request.content">

                        </textarea>
                        <ValidationErrorListComponent v-if="validation.Content" :errors="validation.Content"/>
                    </div>
                    <div>
                        <label class="label">Selected images</label>
                        <div class="is-flex is-flex-wrap-wrap">
                            <figure v-for="img in imageUrls" :key="img" style="width:;" class="image is-128x128 m-1">
                                <img :key="img" :src="img" alt="" />
                            </figure>
                        </div>
                        <ValidationErrorListComponent v-if="validation.Files" :errors="validation.Files"/>
                    </div>
                    <div>
                        <p class="label">
                            Tags:
                        </p>
                        <p>
                            <span v-for="tag in request.tags" :key="tag">#{{ tag }} &nbsp; </span>
                        </p>
                        <ValidationErrorListComponent v-if="validation.Tags" :errors="validation.Tags"/>
                        <div>
                            <input type="text" class="input" v-model="newTag" />
                            <button class="button is-info" @click="addTag">Add tag</button>
                        </div>
                    </div>
                    <div class="control m-2">
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
                    <div class="control m-2">
                        <button class="button is-info" style="width: 100%;" @click="send">Add post</button>
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
import { Ref, reactive, ref } from 'vue';
import { useAuthStore } from '@/store/authStore';
import axios, { AxiosError } from 'axios';
import { useRouter } from 'vue-router';
import ValidationErrorListComponent from '@/components/ValidationErrorListComponent.vue';

const router = useRouter()

const request: AddPostRequest = reactive({
    creatorId: useAuthStore().userId(),
    content: '',
    file: undefined,
    tags: []
})

const validation: Ref<{
    Content?: string[],
    Files?: string[],
    Tags?: string[]
}> = ref({})

const newTag = ref('')

const addTag = () => {
    if (newTag.value == '' || request.tags.includes(newTag.value)) {
        alert("Tag empty or already added!")
        return
    }

    request.tags.push(newTag.value)
    newTag.value = ''
}

const imageUrls: Ref<string[]> = ref([])

const selectFile = (e: Event) => {
    request.files = (e.target! as HTMLInputElement).files!
    
    imageUrls.value = []
    for (let i = 0; i < request.files?.length! ?? 0; i++) {
        imageUrls.value.push(URL.createObjectURL(request.files![i]))
    }
}

const send = () => {
    if (!request.files) {
        alert("No file selected!")
    }

    const formData = new FormData()
    for (let i = 0; i < request.files!.length; i++) {
        formData.append('Files', request.files![i])
    }

    request.tags.forEach(tag => formData.append('Tags[]', tag))

    formData.append('CreatorId', request.creatorId.toString())
    formData.append('Content', request.content)

    axios.post('post', formData).then(() => {
        alert('Post added!')
        router.push('/')
    }).catch((err: AxiosError<ResponseModel>) => {
        validation.value = err.response?.data.validationErrors
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