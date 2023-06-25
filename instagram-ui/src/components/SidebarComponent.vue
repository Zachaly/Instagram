<template>
    <aside class="menu pl-2">
        <p class="menu-label">Navigation</p>
        <ul class="menu-list">
            <li>
                <router-link to="/" active-class="is-active">Main page</router-link>
            </li>
            <li>
                <router-link :to="{ name: 'user', params: { id: authStore.userId() } }" active-class="is-active">
                    Profile
                </router-link>
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
    </aside>
</template>

<script setup lang="ts">
import { MODERATOR_CLAIM } from '@/constants';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
    authStore.logout()
    router.push('/login')
}

</script>