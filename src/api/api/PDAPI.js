
    import { request } from "./request";
    class PDAPI{

    
            GetProductType(params) {
                let objList = ["ProjectCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/GetProductType', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetProductList(params) {
                let objList = ["ProductType","pageIndex","pageSize"];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/GetProductList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetProductDetail(params) {
                let objList = ["ProductCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/GetProductDetail', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            AddRef(params) {
                let objList = [""];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/AddRef', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCardType(params) {
                let objList = ["MemberCode","ProjectCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/GetCardType', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            ProductCodeTrack(params) {
                let objList = ["MemberCode","ProductCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/PD/ProductCodeTrack', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default PDAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    