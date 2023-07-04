export default interface ChangePasswordRequest {
    userId: number,
    oldPassword: string,
    newPassword: string
}