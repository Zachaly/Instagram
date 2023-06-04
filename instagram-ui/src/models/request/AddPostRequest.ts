export default interface AddPostRequest {
    content: string,
    creatorId: number,
    file?: File,
}