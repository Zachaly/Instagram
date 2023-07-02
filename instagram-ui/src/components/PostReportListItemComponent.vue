<template>
    <div class="card">
        <div class="card-content" :class="{ 'has-background-danger': report.accepted }">
            <div class="media">
                <p class="title">
                    <span>
                        <router-link :to="{ name: 'post-report', params: { id: report.id }}">
                            {{ new Date(report.created).toLocaleDateString() }}
                        </router-link>
                    </span>
                    -
                    <span>
                        <router-link :to="{ name: 'user', params: { id: report.reportingUserId } }">
                            {{ report.reportingUserName }}
                        </router-link>
                    </span>
                </p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import PostReportModel from '@/models/PostReportModel';

defineProps<{
    report: PostReportModel
}>()
</script>