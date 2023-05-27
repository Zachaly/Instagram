<template>
    <slot></slot>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/store/authStore";
import { useRouter } from "vue-router";

const authStore = useAuthStore()
const router = useRouter()

if(!authStore.isAuthorized) {
    router.push('/login')
}

</script>
