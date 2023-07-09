<template>
    <AuthorizedPage>
        <NavigationPage>
            <div class="columns is-centered">
                <div class="column is-5">
                    <div class="control">
                        <label class="label">Nickname</label>
                        <input class="input" v-model="request.nickname" />
                        <ValidationErrorListComponent v-if="validation.Nickname" :errors="validation.Nickname"/>
                    </div>
                    <div class="control">
                        <label class="label">Name</label>
                        <input class="input" v-model="request.name" />
                        <ValidationErrorListComponent v-if="validation.Name" :errors="validation.Name"/>
                    </div>
                    <div class="control">
                        <label class="label">Bio</label>
                        <textarea rows="4" class="textarea" v-model="request.bio">
                            
                        </textarea>
                        <ValidationErrorListComponent v-if="validation.Bio" :errors="validation.Bio"/>
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
                    <div style="width: fit-content;">
                        <div class="file is-boxed">
                            <label class="file-label">
                                <input type="file" @change="setFile" class="file-input">
                                <span class="file-cta">
                                    <span class="file-icon">
                                        <font-awesome-icon icon="fa-solid fa-image" />
                                    </span>
                                    <span class="file-label">
                                        Select a new profile picture
                                    </span>
                                </span>
                            </label>
                        </div>
                        <button class="button is-info" style="width: 100%;" @click="sendProfilePicture">Update profile
                            picture</button>
                    </div>
                </div>
                <div class="column is-5">
                    <p class="title">Change password</p>
                    <div class="control">
                        <label class="label">Current password</label>
                        <input type="password" class="input" v-model="changePasswordRequest.oldPassword">
                    </div>
                    <div class="control">
                        <label class="label">New password</label>
                        <input type="password" class="input" v-model="changePasswordRequest.newPassword">
                        <ValidationErrorListComponent v-if="changePasswordValidation.NewPassword" :errors="changePasswordValidation.NewPassword"/>
                    </div>
                    <div class="control">
                        <button class="button is-info" @click="changePassword">Change password</button>
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
import { Ref, onMounted, reactive, toRaw, ref } from 'vue';
import axios, { AxiosError } from 'axios';
import UserModel from '@/models/UserModel';
import NavigationPage from './NavigationPage.vue';
import Gender from '@/models/enum/Gender';
import UpdateProfilePictureRequest from '@/models/request/UpdateProfilePictureRequest';
import ChangePasswordRequest from '@/models/request/ChangePasswordRequest';
import ResponseModel from '@/models/ResponseModel';
import ValidationErrorListComponent from '@/components/ValidationErrorListComponent.vue';

const authStore = useAuthStore()

const request: UpdateUserRequest = reactive({
    id: authStore.userId()
})

const validation: Ref<{
    Name?: string[],
    Nickname?: string[],
    Bio?: string[]
}> = ref({})

const changePasswordValidation: Ref<{ NewPassword?: string[] }> = ref({})

const updatePictureRequest: UpdateProfilePictureRequest = reactive({
    userId: authStore.userId(),
    file: undefined
})

const changePasswordRequest: ChangePasswordRequest = reactive({
    userId: authStore.userId(),
    oldPassword: '',
    newPassword: ''
})

const send = () => {
    axios.patch('user', toRaw(request)).then(() => {
        alert('Profile updated successfully')
        loadUser()
    }).catch((err: AxiosError<ResponseModel>) => {
        validation.value = err.response?.data.validationErrors
    })
}

const changePassword = () => {
    axios.patch('user/change-password', toRaw(changePasswordRequest)).then(res => {
        changePasswordRequest.newPassword = ''
        changePasswordRequest.oldPassword = ''
        alert("Password updated")
    }).catch((err: AxiosError<ResponseModel>) => {
        changePasswordRequest.newPassword = ''
        changePasswordRequest.oldPassword = ''
        changePasswordValidation.value = err.response?.data.validationErrors
        alert(err.response?.data.error)
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