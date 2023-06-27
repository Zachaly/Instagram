<template>
    <NavigationPage>
        <div class="columns is-centered">
            <div class="column is-6">
                <PostCardComponent v-if="post" :post="post" />
            </div>
        </div>
    </NavigationPage>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';
import ResponseModel from '@/models/ResponseModel';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { Ref, onMounted, ref } from 'vue';
import { onBeforeRouteUpdate, useRoute } from 'vue-router';
import NavigationPage from './NavigationPage.vue';
import PostCardComponent from '@/components/PostCardComponent.vue';

const post: Ref<PostModel | null> = ref(null)
const params = useRoute().params

onMounted(() => {
    axios.get<any, AxiosResponse<PostModel>>(`post/${params.id}`).then(res => {
        post.value = res.data
    }).catch((res: AxiosError<ResponseModel>) => {
        console.log(res)
    })
})
onBeforeRouteUpdate(() => {
    axios.get<any, AxiosResponse<PostModel>>(`post/${params.id}`).then(res => {
        post.value = res.data
    }).catch((res: AxiosError<ResponseModel>) => {
        console.log(res)
    })
})

</script>