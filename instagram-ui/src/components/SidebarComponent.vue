<template>
    <aside class="menu pl-2">
        <p class="menu-label">Navigation</p>
        <ul class="menu-list">
            <li>
                <router-link to="/" active-class="is-active">Main page</router-link>
            </li>
            <li>
                <router-link :to="{ name: 'user', params: { id: authStore.userId() } }"
                    active-class="is-active">Profile</router-link>
            </li>
            <li>
                <a class="button is-danger" @click="logout">Logout</a>
            </li>
        </ul>

    </aside>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/store/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
    authStore.logout()
    router.push('/login')
}

</script>