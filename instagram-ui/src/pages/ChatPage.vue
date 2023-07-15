<template>
    <AuthorizedPage>
        <NavigationPage>
            <div v-if="user">
                <p class="title has-text-centered">{{ user.nickname }}</p>
            </div>
            <div>
                <div v-for="message in messages" :key="message.id" class="is-flex" :class="{'is-justify-content-flex-end': authStore.userId() === message.senderId}">
                    <DirectMessageComponent :message="message"/>
                </div>
            </div>
            <div>
                <textarea class="textarea" v-model="newMessageContent" rows="5">
                </textarea>
                <button class="button is-info" @click="sendMessage">Send</button>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/store/authStore';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import { useRoute } from 'vue-router';
import { Ref, onMounted, ref } from 'vue';
import DirectMessageModel from '@/models/DirectMessageModel';
import UserModel from '@/models/UserModel';
import axios, { AxiosResponse } from 'axios';
import GetDirectMessageRequest from '@/models/request/get/GetDirectMessageRequest';
import AddDirectMessageRequest from '@/models/request/AddDirectMessageRequest';
import DirectMessageComponent from '@/components/DirectMessageComponent.vue';
import ResponseModel from '@/models/ResponseModel';
import UpdateDirectMessageRequest from '@/models/request/UpdateDirectMessageRequest';

const authStore = useAuthStore()

const route = useRoute()

const userId = parseInt(route.params['userId'] as string)

const messages: Ref<DirectMessageModel[]> = ref([])
const user: Ref<UserModel | undefined> = ref(undefined)

const newMessageContent = ref('')

const readMessage = (msg: DirectMessageModel) => {
    messages.value.push(msg)

    if(msg.senderId == authStore.userId()){
        return
    }

    msg.read = true
    const patchRequest: UpdateDirectMessageRequest = {
        id: msg.id,
        read: true
    }

    axios.patch('direct-message', patchRequest).then(() => {

    })
}

const sendMessage = () => {
    const request: AddDirectMessageRequest = {
        senderId: authStore.userId(),
        receiverId: userId,
        content: newMessageContent.value
    }

    axios.post('direct-message', request).then((res: AxiosResponse<ResponseModel>) => {
        newMessageContent.value = ''
        axios.get<DirectMessageModel>(`direct-message/${res.data.newEntityId}`).then(r => {
            messages.value.push(r.data)
        })
    }).catch(() => {

    })
}

onMounted(() => {
    axios.get<UserModel>(`user/${userId}`).then(res => {
        user.value = res.data
    })

    const params: GetDirectMessageRequest = {
        UserIds: [userId, authStore.userId()]
    }

    axios.get<DirectMessageModel[]>('direct-message', { params}).then(res => {
        res.data.forEach(msg => {
            readMessage(msg)
        })
    })
})
</script>