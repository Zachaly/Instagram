export default interface AddPostRequest {
    content: string,
    creatorId: number,
    files?: FileList,
    tags: string[]
}