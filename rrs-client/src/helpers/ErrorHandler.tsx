import axios from "axios"
import 'react-toastify/dist/ReactToastify.css';
import { notifyWarning } from "../utils/toastUtils";

export const handleError = (error: any) => {
    if(axios.isAxiosError(error)) {
        var err = error.response;
        if(Array.isArray(err?.data.errors)) {
            for(let val of err?.data.errors) {
                notifyWarning(val.description);
            }
        } else if(typeof err?.data.errors === 'object') {
            for(let e in err?.data.errors) {
                notifyWarning(err.data.errors[e][0]);
            }
        } else if(err?.data) {
            notifyWarning(err.data);
        } else if(err?.status == 401) {
            notifyWarning('Please login');
            window.history.pushState({}, "Login page", "/login");
        } else if(err) {
            notifyWarning(err?.data);
        }
    }
}