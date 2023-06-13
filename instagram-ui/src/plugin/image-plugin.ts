import { API_URL } from "@/constants";
import { App, Plugin } from "vue";

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties {
        $image: (type: string, id: number) => string;
    }
}

export { }

const plugin: Plugin<[]> = {
    install: (app: App<any>) => {
        app.config.globalProperties.$image = (type: string, id: number) => `${API_URL}/image/${type}/${id}`
    }
}

export default plugin