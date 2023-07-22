import PagedRequest from "./PagedRequest";

export default interface GetNotificationRequest extends PagedRequest {
    UserId?: number,
    Created?: number,
    Id?: number,
    Message?: string,
    IsRead?: boolean
}