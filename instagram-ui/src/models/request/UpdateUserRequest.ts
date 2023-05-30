import Gender from "../enum/Gender"

export default interface UpdateUserRequest {
    id: number,
    bio?: string,
    nickname?: string,
    name?: string
    gender?: Gender
}