<template>
  <router-view :key="$route.path"></router-view>
</template>

<script setup lang="ts">
</script>

<style>
@import url('../node_modules/bulma/css/bulma.css');
@import url('../node_modules/bulmaswatch/flatly/bulmaswatch.min.css');

img {
  max-height: 100%;
}

.fixed-center {
  position: fixed;
  top: 20vh;
  left: 40%;
  z-index: 10;
}

.height-100 {
  height: 100%;
}

.width-100 {
  width: 100%;
}

</style>
