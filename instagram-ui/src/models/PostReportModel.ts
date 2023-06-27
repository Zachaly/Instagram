export default interface PostReportModel {
    id: number,
    reportingUserId: number,
    reportingUserName: string,
    postId: number,
    reason: string,
    created: number,
    resolved: boolean,
    resolveTime?: number,
    accepted?: boolean
}