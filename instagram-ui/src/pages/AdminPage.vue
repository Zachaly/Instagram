<template>
    <AuthorizedPage requiredClaim="Admin">
        <NavigationPage>
            <div>Admin</div>
        </NavigationPage>
    </AuthorizedPage>
</template>

<script lang="ts" setup>
import AuthorizedPage from './AuthorizedPage.vue';
import NavigationPage from './NavigationPage.vue'

</script>