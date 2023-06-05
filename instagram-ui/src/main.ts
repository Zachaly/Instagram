import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'
import axios from 'axios'
import { API_URL } from './constants'
import imagePlugin from './plugin/image-plugin'
/* import the fontawesome core */
import { library as fontLibrary, library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import { faImage } from '@fortawesome/free-solid-svg-icons'

library.add(faImage)

const pinia = createPinia()

axios.defaults.baseURL = API_URL

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(pinia)
    .use(router)
    .use(imagePlugin)
    .mount('#app')
