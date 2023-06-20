<template>
    <NavigationPage>
        <div class="columns is-centered">
            <div class="column is-6">
                <div class="is-flex is-flex-wrap-wrap">
                    <PostListItemComponent v-for="post of posts" :key="post.id" :post="post" />
                </div>
            </div>
        </div>
    </NavigationPage>
</template>

<script setup lang="ts">
import PostModel from '@/models/PostModel';
import GetPostRequest from '@/models/request/get/GetPostRequest';
import PostListItemComponent from '@/components/PostListItemComponent.vue';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import NavigationPage from './NavigationPage.vue';

console.log('tag')

const params = useRoute().params

console.log(params)

const posts: Ref<PostModel[]> = ref([])

onMounted(() => {
    const request: GetPostRequest = { SearchTag: params.tag as string }

    axios.get<PostModel[]>("post", { params: request }).then(res => {
        posts.value = res.data
    })
})
</script>