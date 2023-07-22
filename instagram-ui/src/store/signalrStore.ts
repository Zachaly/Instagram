import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from "@microsoft/signalr";
import { defineStore } from "pinia";
import { useAuthStore } from "./authStore";
import { Ref, ref } from "vue";
import { WS_ENDPOINT } from "@/constants";

export const useSignalRStore = defineStore('signalR', () => {
    const authStore = useAuthStore()

    const connections: Ref<{ connection: HubConnection, name: string}[]> = ref([])

    const openConnection = async (name: string): Promise<string> => {
        const existentConnection = connections.value.find(x => x.name == name)

        if(existentConnection) {
            return existentConnection.connection.connectionId!
        }

        const options: IHttpConnectionOptions = {
            headers: { 'Authorization': 'Bearer ' + authStore.token() },
            withCredentials: true,
            accessTokenFactory: () => authStore.token()
        }

        const connection = new HubConnectionBuilder()
            .withUrl(`${WS_ENDPOINT}/${name}`, options)
            .build()

        await connection.start()

        connections.value.push({ connection, name })

        return connection.connectionId!
    }

    const stopAllConnections = () => {
        connections.value.forEach(conn => {
            conn.connection.stop()
        })

        connections.value = []
    }

    const stopConnection = (id: string) => {
        const connection = connections.value.find(x => x.connection.connectionId == id)?.connection
        connection?.stop()
    }

    const addConnectionListener = (id: string, event: string, callback: (...args: any[]) => any) => {
        const connection = connections.value.find(x => x.connection.connectionId == id)?.connection

        connection?.on(event, callback)
    }

    return { openConnection, stopAllConnections, stopConnection, addConnectionListener } 
})