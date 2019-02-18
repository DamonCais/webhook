
    import { request } from "./request";
    class DictAPI{

    
            QueryProject(params) {
                let objList = ["Online"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Dict/QueryProject', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default DictAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    