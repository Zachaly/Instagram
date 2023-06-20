import PagedRequest from "./PagedRequest";

export default interface GetUserRequest extends PagedRequest {
    Id?: number,
    Nickname?: string,
    Name?: string,
    UserIds?: number[],
    SkipIds?: number[],
    SearchNickname?: string
}