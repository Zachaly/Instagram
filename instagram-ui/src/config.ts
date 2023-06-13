import axios from "axios";
import QueryString from "qs";
import { API_URL } from "./constants";
import { faImage, faCaretLeft, faCaretRight, faCircle, faCircleNotch, faX, faHeart } from '@fortawesome/free-solid-svg-icons'
import { library } from '@fortawesome/fontawesome-svg-core'

export const axiosConfig = () => {
    axios.defaults.baseURL = API_URL
    axios.defaults.paramsSerializer = params => QueryString.stringify(params)
}

export const fontAwesomeConfig = () => {
    library.add(faImage, faCaretLeft, faCaretRight, faCircle, faCircleNotch, faX, faHeart)
}