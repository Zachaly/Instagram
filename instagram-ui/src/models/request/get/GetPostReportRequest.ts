export default interface GetPostReportRequest {
    Id?: number,
    ReportingUserId?: number,
    PostId?: number,
    Reason?: string,
    Created?: number,
    Resolved?: number,
    ResolveTime?: number
}