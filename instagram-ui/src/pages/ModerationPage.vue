<template>
    <AuthorizedPage :allowed-claims="[ADMIN_CLAIM, MODERATOR_CLAIM]">
        <NavigationPage :hide-search="true">
            <TabsComponent :names="['Unresolved', 'Resolved', 'Bans']" @select="selectIndex" />
            <div v-if="currentIndex == 0 || currentIndex == 1">
                <PostReportListItemComponent  v-for="report in reports" :key="report.id" :report="report" />
            </div>
            <div v-else-if="currentIndex == 2">
                <BanListComponent/>
            </div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { ADMIN_CLAIM, MODERATOR_CLAIM } from '@/constants';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';
import axios from 'axios';
import { Ref, onMounted, ref } from 'vue';
import TabsComponent from '@/components/TabsComponent.vue';
import PostReportModel from '@/models/PostReportModel';
import GetPostReportRequest from '@/models/request/get/GetPostReportRequest';
import PostReportListItemComponent from '@/components/PostReportListItemComponent.vue';
import BanListComponent from '@/components/BanListComponent.vue';

const currentIndex = ref(0)

const reports: Ref<PostReportModel[]> = ref([])

const selectIndex = (index: number) => {
    currentIndex.value = index
    reports.value = []
    if (index == 0 || index == 1) {
        loadReports()
    }
}

const loadReports = () => {
    const params: GetPostReportRequest = {}

    if (currentIndex.value == 0) {
        params.Resolved = false
    } else if (currentIndex.value == 1) {
        params.Resolved = true
    }

    axios.get<PostReportModel[]>('post-report', { params }).then(res => {
        reports.value = res.data
    })
}

onMounted(() => {
    loadReports()
})
</script>