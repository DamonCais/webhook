
    import { request } from "./request";
    class SysAPI{

    
            GetVersionList(params) {
                let objList = ["ver"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/GetVersionList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SendSms(params) {
                let objList = ["mobile"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/SendSms', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            code2Session(params) {
                let objList = ["appid","js_code"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/code2Session', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            AES_decrypt(params) {
                let objList = ["encryptedDataStr","key","iv"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/AES_decrypt', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SaveFile(params) {
                let objList = ["fileName"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/SaveFile', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SaveImage(params) {
                let objList = ["ImageName","width","height"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/SaveImage', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            WxPaySign(params) {
                let objList = ["MemberCode","AppID","openid","CardNo","type"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/WxPaySign', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            PayBalance(params) {
                let objList = ["MemberCode","CardNo"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Sys/PayBalance', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default SysAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    