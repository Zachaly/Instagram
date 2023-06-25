<template>
    <AuthorizedPage :allowed-claims="['Admin', MODERATOR_CLAIM]">
        <NavigationPage :hide-search="true">
            Moderation
        </NavigationPage>
    </AuthorizedPage>
</template>

<script setup lang="ts">
import { MODERATOR_CLAIM } from '@/constants';
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue';


</script>