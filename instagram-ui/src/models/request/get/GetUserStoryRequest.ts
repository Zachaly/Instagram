import PagedRequest from "./PagedRequest";

export default interface GetUserStoryRequest extends PagedRequest {
    UserId?: number,
    Id?: number,
    Created?: number,
    UserIds?: number[]
}