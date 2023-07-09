<template>
    <span v-for="error in errors" :key="error" class="help is-warning">{{ error }}</span>
</template>

<script setup lang="ts">

defineProps<{
    errors: string[]
}>()
</script>