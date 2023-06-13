import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'
import imagePlugin from './plugin/image-plugin'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { axiosConfig, fontAwesomeConfig } from './config'

axiosConfig()
fontAwesomeConfig()

const pinia = createPinia()

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(pinia)
    .use(router)
    .use(imagePlugin)
    .mount('#app')
