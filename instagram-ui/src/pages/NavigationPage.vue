<template>
    <div class="columns mt-2">
        <div class="column is-2" v-if="authStore.isAuthorized">
            <SidebarComponent />
        </div>
        <div class="column">
            <slot></slot>
        </div>
    </div>
</template>

<script setup lang="ts">
import SidebarComponent from '@/components/SidebarComponent.vue';
import { useAuthStore } from '@/store/authStore';

const authStore = useAuthStore()

</script>