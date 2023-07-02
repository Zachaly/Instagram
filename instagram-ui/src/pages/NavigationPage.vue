<template>
    <div class="columns mt-2">
        <div class="column is-2" v-if="authStore.isAuthorized">
            <SidebarComponent />
        </div>
        <div class="column">
            <div class="is-flex is-justify-content-center" v-if="!hideSearch">
                <SearchBarComponent />
            </div>
            <slot></slot>
        </div>
    </div>
</template>

<script setup lang="ts">
import SearchBarComponent from '@/components/SearchBarComponent.vue';
import SidebarComponent from '@/components/SidebarComponent.vue';
import { useAuthStore } from '@/store/authStore';

const authStore = useAuthStore()
defineProps<{
    hideSearch?: boolean
}>()
</script>