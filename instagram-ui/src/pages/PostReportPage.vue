<template>
    <AuthorizedPage :allowed-claims="['Admin', MODERATOR_CLAIM]">
        <NavigationPage :hide-search="true">
            <div class="columns is-centered">
                <div class="column is-5">
                    <div v-if="report">
                        <PostReportListItemComponent :report="report" v-if="report" />
                        <p class="subtitle has-text-centered is-3 mt-2">
                            <b>Reason</b>: {{ report.reason }}
                        </p>
                        <PostCardComponent v-if="post" :post="post" />
                    </div>
                </div>
                <div class="column is-1" v-if="!report?.resolved">
                    <div class="control">
                        <label class="label">Ban end date</label>
                        <input type="date" @change="setDate" class="input">
                    </div>
                    <button class="button is-danger width-100" @click="respond(true)">Accept</button>
                    <button class="button is-warning width-100" @click="respond(false)">Deny</button>
                </div>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { MODERATOR_CLAIM } from '@/constants';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import { Ref, onMounted, ref } from 'vue';
import PostReportModel from '@/models/PostReportModel';
import PostModel from '@/models/PostModel';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';
import PostReportListItemComponent from '@/components/PostReportListItemComponent.vue';
import PostCardComponent from '@/components/PostCardComponent.vue';
import ResolvePostReportRequest from '@/models/request/ResolvePostReportRequest';

const report: Ref<PostReportModel | undefined> = ref(undefined)
const post: Ref<PostModel | undefined> = ref(undefined)

const router = useRouter()

const params = useRoute().params

const banEndDate = ref(0)

const setDate = (e: Event) => {
    const timeStamp = Date.parse((e.target as HTMLInputElement).value)
    console.log(timeStamp)
    if (timeStamp < Date.now()) {
        alert('Invalid date!')
        return
    }
    banEndDate.value = timeStamp
}

const respond = (accepted: boolean) => {
    const request: ResolvePostReportRequest = {
        id: report.value!.id,
        accepted,
        postId: report.value!.postId,
        banEndDate: banEndDate.value,
        userId: post.value?.creatorId
    }

    axios.put('post-report/resolve', request).then(res => {
        router.push('/moderation')
    })
}

onMounted(() => {
    axios.get<PostReportModel>(`post-report/${params.id}`).then(res => {
        report.value = res.data

        axios.get<PostModel>(`post/${report.value.postId}`).then(res => {
            post.value = res.data
        })
    })
})
</script>