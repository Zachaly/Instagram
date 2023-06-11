export default interface GetUserRequest {
    Id?: number,
    Nickname?: string,
    Name?: string,
    UserIds?: number[],
    SkipIds?: number[]
}