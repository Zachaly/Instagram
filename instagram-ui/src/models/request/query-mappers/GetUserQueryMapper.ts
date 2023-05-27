import GetUserRequest from "../GetUserRequest";

export default (request: GetUserRequest) : URLSearchParams => {
    const queryParams = new URLSearchParams()
    
    if(request.id){
        queryParams.append('Id', request.id.toString())
    }

    if(request.name){
        queryParams.append('Name', request.name)
    }

    if(request.nickname){
        queryParams.append('Nickname', request.nickname)
    }

    return queryParams
}