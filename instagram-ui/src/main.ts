import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'
import axios from 'axios'
import { API_URL } from './constants'
import imagePlugin from './plugin/image-plugin'
const pinia = createPinia()

axios.defaults.baseURL = API_URL

createApp(App).use(pinia).use(router).use(imagePlugin).mount('#app')
