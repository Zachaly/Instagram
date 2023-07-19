import PagedRequest from "./PagedRequest";

export default interface GetAccountVerificationRequest extends PagedRequest {
    Id?: number,
    Created?: number,
    FirstName?: string,
    LastName?: string,
    DateOfBirth?: string
}