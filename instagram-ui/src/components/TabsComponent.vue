<template>
    <div class="tabs is-centered">
        <ul>
            <li :class="{ 'is-active': selectedIndex == index }" v-for="(name, index) in names" :key="index" @click="selectIndex(index)">
                <a href="#">{{ name }}</a>
            </li>
        </ul>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    names: string[]
}>()

const selectedIndex = ref(0)

const emit = defineEmits<{
    (e: 'select', index: number): void
}>()

const selectIndex = (index: number) => {
    selectedIndex.value = index
    emit('select', index)
}
</script>