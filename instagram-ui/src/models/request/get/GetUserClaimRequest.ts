import PagedRequest from "./PagedRequest";

export default interface GetUserClaimRequest extends PagedRequest {
    UserId?: number,
    Value?: string
}