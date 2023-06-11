import PagedRequest from "./PagedRequest";

export default interface GetUserFollowRequest extends PagedRequest {
    FollowingUserId?: number,
    FollowedUserId?: number,
    JoinFollower?: boolean,
    JoinFollowed?: boolean
}