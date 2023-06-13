<template>
    <NavigationPage>
        <div class="columns is-centered">
            <div class="column is-6">
                <UserCardComponent v-if="!loading" :user="user" />
                <p class="title">Posts</p>
                <div class="is-flex is-flex-wrap-wrap">
                    <PostListItemComponent v-for="post of posts" :key="post.id" :post="post" />
                </div>
            </div>
        </div>
    </NavigationPage>
</template>

<script setup lang="ts">
import PostListItemComponent from '@/components/PostListItemComponent.vue';
import NavigationPage from './NavigationPage.vue';
import UserCardComponent from '@/components/UserCardComponent.vue';
import PostModel from '@/models/PostModel';
import UserModel from '@/models/UserModel';
import Gender from '@/models/enum/Gender';
import GetPostRequest from '@/models/request/get/GetPostRequest';
import axios from 'axios';
import { Ref, onMounted, reactive, ref } from 'vue';
import { useRoute } from 'vue-router'

const params = useRoute().params
const loading: Ref<boolean> = ref(true)
const user: UserModel = reactive({
    name: '',
    nickname: '',
    bio: '',
    id: 0,
    gender: Gender.NotSpecified,
})

const posts: Ref<PostModel[]> = ref([])

onMounted(() => {
    axios.get<UserModel>('/user/' + params.id).then(res => {
        Object.assign(user, res.data)
        loading.value = false
    })

    const getPostsRequest: GetPostRequest = {
        CreatorId: parseInt(params.id as string)
    }

    axios.get<PostModel[]>('post', { params: getPostsRequest }).then(res => {
        posts.value = res.data
    })
})

</script>