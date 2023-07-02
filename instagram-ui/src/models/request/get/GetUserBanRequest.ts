import PagedRequest from "./PagedRequest";

export default interface GetUserBanRequest extends PagedRequest {
   Id?: number,
   StartDate?: number,
   EndDate?: number,
   UserId?: number,
   MinEndDate?: number,
}