
    import { request } from "./request";
    class IoTAPI{

    
            RegisterDevice(params) {
                let objList = ["productKey","deviceName"];
                params=_.pick(params,objList);
                const href = getFullUrl('/IoT/RegisterDevice', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default IoTAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    