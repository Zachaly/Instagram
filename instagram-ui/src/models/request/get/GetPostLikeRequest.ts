import PagedRequest from "./PagedRequest";

export default interface GetPostLikeRequest extends PagedRequest {
    UserId?: number,
    PostId?: number
}