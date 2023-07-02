import PagedRequest from "./PagedRequest";

export default interface GetPostCommentRequest extends PagedRequest {
    UserId?: number,
    PostId?: number,
    Id?: number
}