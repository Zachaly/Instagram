import Gender from "./enum/Gender";

export default interface UserModel {
    id: number,
    name: string,
    nickname: string,
    bio: string,
    gender: Gender,
    verified: boolean
}