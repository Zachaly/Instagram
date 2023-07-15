import PagedRequest from "./PagedRequest";

export default interface GetDirectMessageRequest extends PagedRequest {
    Id?: number,
    SenderId?: number,
    ReceiverId?: number,
    Created?: number,
    Read?: boolean,
    Content?: string,
    UserIds?: number[]
}