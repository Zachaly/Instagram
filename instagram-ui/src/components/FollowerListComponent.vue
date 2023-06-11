<template>
    <div class="box" style="min-height: 40vh; min-width: 30vh;">
        <div class="has-text-centered">
            <font-awesome-icon @click="$emit('close')" :icon="['fas', 'x']" />
        </div>
        <UserLinkComponent v-for="user in follows" :key="user.followedUserId + user.followingUserId"
            :id="follower ? user.followingUserId : user.followedUserId" :nick-name="user.userName!" />
    </div>
</template>

<script setup lang="ts">
import UserFollowModel from '@/models/UserFollowModel';
import UserLinkComponent from './UserLinkComponent.vue';

const props = defineProps<{
    follows: UserFollowModel[],
    follower: boolean
}>()

defineEmits(['close'])

</script>