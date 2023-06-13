import PagedRequest from "./PagedRequest"

export default interface GetPostRequest extends PagedRequest {
    CreatorId?: number,
    Id?: number
    CreatorIds?: number[]
}