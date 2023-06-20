<template>
    <span class="has-background-gray" style="width: fit-content;">
        <router-link :to="{ name: 'tag', params: { tag }}"> #{{ tag }}</router-link> &nbsp;
    </span>
</template>

<script lang="ts" setup>

defineProps<{
    tag: string
}>()
</script>