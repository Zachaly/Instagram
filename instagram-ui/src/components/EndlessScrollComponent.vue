<template>
    <div class="endless-scroll" :class="$props.class ?? ''" @scroll="onScroll" :style="style" ref="box">
        <slot></slot>
    </div>
</template>

<script lang="ts" setup>
import { Ref, onMounted, ref } from 'vue';


const props = defineProps<{
    class?: string,
    style?: string,
}>()

const emit = defineEmits(['on-top', 'on-bottom'])

const box: Ref<HTMLDivElement | null> = ref(null)

const onScroll = (e: Event) => {
    const target = e.target as HTMLDivElement

    if (target.scrollTop < target.scrollHeight / 20) {
        emit('on-top')
    }

    if (target.scrollTop > (target.scrollHeight - target.clientHeight) * 0.8) {
        emit('on-bottom')
    }
}

</script>


<style>
.endless-scroll {
    overflow-y: scroll;
}

.endless-scroll::-webkit-scrollbar {
    width: 10px;
}

.endless-scroll::-webkit-scrollbar-thumb {
    border: 1px;
    background: #888;
}
</style>
