export default interface UserStoryModel {
    userId: number,
    userName: string,
    images: UserStoryImage[]
}

export interface UserStoryImage {
    id: number,
    created: number
}