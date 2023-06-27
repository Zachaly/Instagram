export default interface GetPostReportRequest {
    Id?: number,
    ReportingUserId?: number,
    PostId?: number,
    Reason?: string,
    Created?: number,
    Resolved?: boolean,
    ResolveTime?: number
}