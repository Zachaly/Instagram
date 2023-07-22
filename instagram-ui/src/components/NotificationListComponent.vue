<template>
    <div>
        <p class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </p>
        <div class="has-text-centered">
            <button class="button is-info m-1" @click="readAll">Read all</button>
            <button class="button is-info m-1" @click="clear">Clear</button>
        </div>
        <NotificationComponent v-for="notification in notifications" :notification="notification" :key="notification.id"
            @read="read" @delete="remove" />
    </div>
</template>

<script setup lang="ts">
import { Ref, onMounted, ref } from 'vue';
import NotificationComponent from './NotificationComponent.vue';
import NotificationModel from '@/models/NotificationModel';
import { useAuthStore } from '@/store/authStore';
import { useSignalRStore } from '@/store/signalrStore';
import GetNotificationRequest from '@/models/request/get/GetNotificationRequest';
import axios from 'axios';
import UpdateNotificationRequest from '@/models/request/UpdateNotificationRequest';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const authStore = useAuthStore()
const signalR = useSignalRStore()

defineEmits(['close'])

const notifications: Ref<NotificationModel[]> = ref([])

const startListening = async () => {
    const connectionId = await signalR.openConnection('notification')

    signalR.addConnectionListener(connectionId, 'NotificationReceived', (notification: NotificationModel) => {
        notifications.value.unshift(notification)
    })
}

const read = (id: number) => {
    const request: UpdateNotificationRequest = {
        id,
        isRead: true
    }

    axios.patch('notification', request).then(() => {
        const notification = notifications.value.find(x => x.id == id)
        if (notification) {
            notification.isRead = true
        }
    })
}

const remove = (id: number) => {
    axios.delete(`notification/${id}`).then(() => {
        notifications.value = notifications.value.filter(x => x.id !== id)
    })
}

const readAll = () => {
    notifications.value.filter(x => !x.isRead).forEach(notification => {
        read(notification.id)
    })
}

const clear = () => {
    notifications.value.forEach(x => {
        axios.delete(`notification/${x.id}`).then(() => {

        })
    })

    notifications.value = []
}

onMounted(() => {
    const params: GetNotificationRequest = {
        UserId: authStore.userId(),
    }

    startListening()

    axios.get<NotificationModel[]>('notification', { params }).then(res => {
        notifications.value = res.data
    })
})

</script>