<template>
    <AuthorizedPage requiredClaim="Admin">
        <NavigationPage :hideSearch="true">
            <TabsComponent :names="['Moderators', 'Add moderator']" @select="selectIndex"/>
            <ModeratorListComponent v-if="currentIndex == 0"/>
            <AddModeratorComponent v-else-if="currentIndex == 1"/>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script lang="ts" setup>
import TabsComponent from '@/components/TabsComponent.vue';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue'
import ModeratorListComponent from '@/components/ModeratorListComponent.vue'
import AddModeratorComponent from '@/components/AddModeratorComponent.vue'
import { ref } from 'vue';

const currentIndex = ref(0)

const selectIndex = (index: number) => currentIndex.value = index

</script>