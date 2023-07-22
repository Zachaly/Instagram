<template>
    <aside class="menu pl-2">
        <p class="menu-label">Navigation</p>
        <ul class="menu-list" v-if="!showNotifications">
            <li>
                <router-link to="/" active-class="is-active">Main page</router-link>
            </li>
            <li>
                <router-link :to="{ name: 'user', params: { id: authStore.userId() } }" active-class="is-active">
                    Profile
                </router-link>
            </li>
            <li>
                <a href="#" @click="showNotifications = true">
                    Notifications <span v-if="notificationCount > 0" class="has-text-danger">({{ notificationCount }})</span>
                </a>
            </li>
            <li>
                <router-link to="/add-post" active-class="is-active">
                    Add post
                </router-link>
            </li>
            <li v-if="authStore.hasClaim('Admin')">
                <router-link to="/admin" active-class="is-active">
                    Admin
                </router-link>
            </li>
            <li v-if="authStore.hasClaim('Admin') || authStore.hasClaim(MODERATOR_CLAIM)">
                <router-link to="/moderation" active-class="is-active">
                    Moderation
                </router-link>
            </li>
            <li>
                <a class="button is-danger" @click="logout">Logout</a>
            </li>
        </ul>
        <div v-else>
            <NotificationListComponent @close="showNotifications = false" />
        </div>
    </aside>
</template>

<script setup lang="ts">
import { MODERATOR_CLAIM } from '@/constants';
import NotificationModel from '@/models/NotificationModel';
import GetNotificationRequest from '@/models/request/get/GetNotificationRequest';
import { useAuthStore } from '@/store/authStore';
import { useSignalRStore } from '@/store/signalrStore';
import axios from 'axios';
import { onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import NotificationListComponent from './NotificationListComponent.vue';

const authStore = useAuthStore()
const router = useRouter()
const signalR = useSignalRStore()

const notificationCount = ref(0)
const showNotifications = ref(false)

const logout = () => {
    signalR.stopAllConnections()
    authStore.logout()
    router.push('/login')
}

const startListening = async () => {
    const connectionId = await signalR.openConnection('notification')

    signalR.addConnectionListener(connectionId, 'NotificationReceived', (notification: NotificationModel) => {
        notificationCount.value++
    })
}

const loadNotificationsCount = () => {
    const params: GetNotificationRequest = {
        IsRead: false,
        UserId: authStore.userId()
    }

    axios.get<number>('notification/count', { params }).then(res => {
        notificationCount.value = res.data
    })
}

watch(showNotifications, () => {
    loadNotificationsCount()
})

onMounted(() => {
    loadNotificationsCount()
    startListening()
})
</script>