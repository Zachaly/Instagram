<template>
    <div class="media box m-1 has-background-grey-lighter p-1">
        <div class="media-left">
            <figure class="image is-48x48">
                <img :src="$image('profile', id)" class="is-rounded">
            </figure>
        </div>
        <div class="media-content" style="margin: auto auto;">
            <p class="title is-5">
                <router-link :to="{ name: 'user', params: { id } }">
                    {{ nickName }} <font-awesome-icon :icon="['fas', 'check']" v-if="verified"/>
                </router-link>
            </p>
        </div>
    </div>
</template>

<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const props = defineProps<{
    nickName: string,
    id: number,
    verified?: boolean
}>()
</script>

<style scoped>
div {
    user-select: none;
}
</style>