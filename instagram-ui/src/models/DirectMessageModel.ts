export default interface DirectMessageModel {
    id: number,
    senderId: number,
    created: number,
    read: boolean,
    content: string
}