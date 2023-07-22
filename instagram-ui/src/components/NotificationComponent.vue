<template>
    <div class="box" :class="{ 'has-background-info': !notification.isRead }" @click="$emit('read', notification.id)">
        {{ notification.message }} - {{ time }} <font-awesome-icon @click="$emit('delete', notification.id)" :icon="['fas', 'x']" />
    </div>
</template>

<script lang="ts" setup>
import NotificationModel from '@/models/NotificationModel';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed } from 'vue';

const props = defineProps<{
    notification: NotificationModel
}>()

defineEmits(['read', 'delete'])

const time = computed(() => {
    const timePassed = Math.floor((Date.now() - props.notification.created) / 1000) //convert to seconds

    if(timePassed < 60){
        return 'now'
    }
    else if(timePassed < 3600) {
        return `${Math.floor(timePassed / 60)} minutes ago`
    }

    return `${Math.floor(timePassed / 3600)} hours ago`
})

</script>