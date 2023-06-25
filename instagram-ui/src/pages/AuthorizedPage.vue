<template>
    <slot></slot>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/store/authStore";
import { useRouter } from "vue-router";

const authStore = useAuthStore()
const router = useRouter()

const props = defineProps<{
    requiredClaim?: string
}>()

if(!authStore.isAuthorized && (!props.requiredClaim || !authStore.hasClaim(props.requiredClaim))) {
    router.push('/login')
}

</script>
