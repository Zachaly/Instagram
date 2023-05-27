import Gender from '../enum/Gender'

export default interface RegisterRequest {
    name: string,
    email: string,
    password: string,
    nickname: string,
    gender: Gender
}