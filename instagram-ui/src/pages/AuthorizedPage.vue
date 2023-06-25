<template>
    <slot></slot>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/store/authStore";
import { useRouter } from "vue-router";

const authStore = useAuthStore()
const router = useRouter()

const props = defineProps<{
    allowedClaims?: string[]
}>()

if(!authStore.isAuthorized) {
    router.push('/login')
} else if(props.allowedClaims && !props.allowedClaims.some(claim => authStore.hasClaim(claim))) {
    router.push('/')
}

</script>
