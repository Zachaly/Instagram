import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'
import axios from 'axios'
import { API_URL } from './constants'
import imagePlugin from './plugin/image-plugin'
import { library as  library } from '@fortawesome/fontawesome-svg-core'

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import { faImage, faCaretLeft, faCaretRight, faCircle, faCircleNotch, faX } from '@fortawesome/free-solid-svg-icons'
import QueryString from 'qs'

library.add(faImage, faCaretLeft, faCaretRight, faCircle, faCircleNotch, faX)

const pinia = createPinia()

axios.defaults.baseURL = API_URL
axios.defaults.paramsSerializer = params => QueryString.stringify(params)

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(pinia)
    .use(router)
    .use(imagePlugin)
    .mount('#app')
