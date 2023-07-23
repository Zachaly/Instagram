<template>
    <AuthorizedPage>
        <NavigationPage>
            <div v-if="user">
                <p class="title has-text-centered">{{ user.nickname }}</p>
            </div>
            <EndlessScrollComponent class="m-1" :style="'max-height: 60vh;'" @on-top="onTop">
                <div v-for="message in messages" :key="message.id" class="is-flex"
                    :class="{ 'is-justify-content-flex-end': authStore.userId() === message.senderId }">
                    <DirectMessageComponent :message="message" />
                </div>
            </EndlessScrollComponent>
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
import { Ref, onMounted, onUnmounted, ref } from 'vue';
import DirectMessageModel from '@/models/DirectMessageModel';
import UserModel from '@/models/UserModel';
import axios, { AxiosResponse } from 'axios';
import GetDirectMessageRequest from '@/models/request/get/GetDirectMessageRequest';
import AddDirectMessageRequest from '@/models/request/AddDirectMessageRequest';
import DirectMessageComponent from '@/components/DirectMessageComponent.vue';
import ResponseModel from '@/models/ResponseModel';
import UpdateDirectMessageRequest from '@/models/request/UpdateDirectMessageRequest';
import EndlessScrollComponent from '@/components/EndlessScrollComponent.vue';
import { useSignalRStore } from '@/store/signalrStore';

const authStore = useAuthStore()
const signalR = useSignalRStore()
const connectionId = ref('')

const route = useRoute()

const userId = parseInt(route.params['userId'] as string)

const messages: Ref<DirectMessageModel[]> = ref([])
const user: Ref<UserModel | undefined> = ref(undefined)
const blockScroll = ref(false)

const PAGE_SIZE = 10;
const currentPageIndex = ref(0)

const newMessageContent = ref('')

const readMessage = (msg: DirectMessageModel, push: boolean = true) => {
    if (push) {
        messages.value.push(msg)
    } else {
        messages.value.unshift(msg)
    }

    if (msg.senderId == authStore.userId()) {
        return
    }

    msg.read = true
    const patchRequest: UpdateDirectMessageRequest = {
        id: msg.id,
        read: true
    }

    axios.patch('direct-message', patchRequest).then()
}

const sendMessage = () => {
    const request: AddDirectMessageRequest = {
        senderId: authStore.userId(),
        receiverId: userId,
        content: newMessageContent.value
    }

    axios.post('direct-message', request).then((res: AxiosResponse<ResponseModel>) => {
        newMessageContent.value = ''
    }).catch()
}

const loadMessages = () => {
    blockScroll.value = true

    const params: GetDirectMessageRequest = {
        UserIds: [userId, authStore.userId()],
        PageIndex: currentPageIndex.value,
        PageSize: PAGE_SIZE
    }

    axios.get<DirectMessageModel[]>('direct-message', { params }).then(res => {
        currentPageIndex.value++
        blockScroll.value = res.data.length < PAGE_SIZE

        res.data.forEach(msg => {
            readMessage(msg, false)
        })
    })
}

const onTop = () => {
    if (!blockScroll.value) {
        loadMessages()
    }
}

const startListening = async () => {
    const id = await signalR.openConnection('direct-message')

    signalR.addConnectionListener(id, 'MessageReceived', (msg: DirectMessageModel) => {
        if (msg.senderId == userId || msg.senderId == authStore.userId()) {
            readMessage(msg, true)
        }
    })

    signalR.addConnectionListener(id, 'MessageRead', (id: number, isRead: boolean) => {
        const msg = messages.value.find(x => x.id == id)
        if (msg) {
            msg.read = isRead
        }
    })

    connectionId.value = id
}

onMounted(() => {
    axios.get<UserModel>(`user/${userId}`).then(res => {
        user.value = res.data
    })

    loadMessages()

    startListening()
})

onUnmounted(() => {
    console.log('unmyn')
    signalR.stopConnection(connectionId.value)
})
</script>