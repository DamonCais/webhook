
import { request } from "./request";
class ADVAPI {


    AdvHeadCreate(params) {
        let objList = ["AdCode", "CorpCode", "ADTitle", "AdType", "SubmitBy"];
        params = _.pick(params, objList);
        const href = getFullUrl('/ADV/AdvHeadCreate', params);
        return request({
            url: href,
            method: 'get'
        })
    }


    AdvListCreate(params) {
        let objList = ["AdCode", "SortID", "OnLine", "AdLink", "AdContents", "AdImageLink", "AdImageURL", "AdImageTitle"];
        params = _.pick(params, objList);
        const href = getFullUrl('/ADV/AdvListCreate', params);
        return request({
            url: href,
            method: 'get'
        })
    }


    AdvHeadRevise(params) {
        let objList = ["AdCode", "ADTitle", "AdType", "SubmitBy", "isDefault"];
        params = _.pick(params, objList);
        const href = getFullUrl('/ADV/AdvHeadRevise', params);
        return request({
            url: href,
            method: 'get'
        })
    }


    AdvListRevise(params) {
        let objList = ["AdCode", "SortID", "OnLine", "AdLink", "AdContents", "AdImageLink", "AdImageURL", "AdImageTitle"];
        params = _.pick(params, objList);
        const href = getFullUrl('/ADV/AdvListRevise', params);
        return request({
            url: href,
            method: 'get'
        })
    }


    GetAdvInfo(params) {
        let objList = ["AdCode"];
        params = _.pick(params, objList);
        const href = getFullUrl('/ADV/GetAdvInfo', params);
        return request({
            url: href,
            method: 'get'
        })
    }


}
export default ADVAPI;
function getFullUrl(url, params) {
    let query = "";
    for (let p in params) {
        let str = '&' + p + '=' + params[p]
        query += str;
    }
    url += "?" + query.substring(1);
    return url + '&token=00001iloveyouruo';
}
