import PagedRequest from "./PagedRequest";

export default interface GetUserBlockRequest extends PagedRequest {
    Id?: number,
    BlockingUserId?: number,
    BlockedUserId?: number
}