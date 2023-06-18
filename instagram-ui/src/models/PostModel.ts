export default interface PostModel {
    id: number,
    creatorId: number,
    creatorName: string,
    content: string,
    created: number,
    imageIds: number[],
    likeCount: number,
    commentCount: number,
    tags: string[]
}