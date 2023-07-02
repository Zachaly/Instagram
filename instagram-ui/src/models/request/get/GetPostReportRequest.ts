import PagedRequest from "./PagedRequest";

export default interface GetPostReportRequest extends PagedRequest {
    Id?: number,
    ReportingUserId?: number,
    PostId?: number,
    Reason?: string,
    Created?: number,
    Resolved?: boolean,
    ResolveTime?: number
}