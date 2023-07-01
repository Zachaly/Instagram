export default interface ResolvePostReportRequest {
    id: number,
    postId: number,
    accepted: boolean,
    userId?: number,
    banEndDate?: number
}