<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns is-centered">
                <div class="column is-5">
                    <div class="control">
                        <label class="label">Nickname</label>
                        <input class="input" v-model="request.nickname" />
                    </div>
                    <div class="control">
                        <label class="label">Name</label>
                        <input class="input" v-model="request.name" />
                    </div>
                    <div class="control">
                        <label class="label">Bio</label>
                        <textarea rows="4" class="textarea" v-model="request.bio">

                        </textarea>
                    </div>
                    <div class="select mt-1">
                        <select class="select" v-model="request.gender">
                            <option :value="Gender.NotSpecified">Not specified</option>
                            <option :value="Gender.Man">Man</option>
                            <option :value="Gender.Woman">Woman</option>
                        </select>
                    </div>
                    <div class="buttons m-1">
                        <button class="button is-info" @click="send">
                            Confirm changes
                        </button>
                        <button class="button is-warning" @click="loadUser">
                            Reset changes
                        </button>
                    </div>

                    <div class="control">
                        <div class="file is-boxed">
                            <label class="file-label">
                                <input type="file" @change="setFile" class="file-input">
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
                        <button class="button is-info" @click="sendProfilePicture">Update profile picture</button>
                    </div>

                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/store/authStore';
import AuthorizedPage from './AuthorizedPage.vue';
import UpdateUserRequest from '@/models/request/UpdateUserRequest';
import { onMounted, reactive, toRaw } from 'vue';
import axios from 'axios';
import UserModel from '@/models/UserModel';
import NavigationPage from './NavigationPage.vue';
import Gender from '@/models/enum/Gender';
import UpdateProfilePictureRequest from '@/models/request/UpdateProfilePictureRequest';

const authStore = useAuthStore()

const request: UpdateUserRequest = reactive({
    id: authStore.userId()
})

const updatePictureRequest: UpdateProfilePictureRequest = reactive({
    userId: authStore.userId(),
    file: undefined
})

const send = () => {
    axios.patch('user', toRaw(request)).then(() => {
        alert('Profile updated successfully')
        loadUser()
    })
}

const setFile = (e: Event) => {
    updatePictureRequest.file = (e.target! as HTMLInputElement).files![0]
}

const sendProfilePicture = () => {
    const formData = new FormData()
    formData.append('File', updatePictureRequest.file ?? "")
    formData.append('UserId', authStore.userId().toString())

    axios.patch('image/profile', formData).then(res => alert('Profile picture updated!'))
}

const loadUser = () => {
    axios.get<UserModel>('user/' + authStore.userId()).then(res => {
        request.name = res.data.name
        request.bio = res.data.bio
        request.gender = res.data.gender
        request.nickname = res.data.nickname
    })
}

onMounted(() => {
    loadUser()
})

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