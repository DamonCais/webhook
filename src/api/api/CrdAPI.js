
    import { request } from "./request";
    class CrdAPI{

    
            BalanceRecharge(params) {
                let objList = ["BalanceID","MemberCode","PayWay"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Crd/BalanceRecharge', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            BalanceTypeCreate(params) {
                let objList = ["BalanceID","Description","OriginalAmount","DonationAmount","IsOnline","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Crd/BalanceTypeCreate', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetBalanceTypeList(params) {
                let objList = ["IsOnline"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Crd/GetBalanceTypeList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SetBalanceType(params) {
                let objList = ["BalanceID","Description","OriginalAmount","DonationAmount","IsOnline","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Crd/SetBalanceType', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            AddCredits(params) {
                let objList = ["MemberCode","CreditsID","Remark"];
                params=_.pick(params,objList);
                const href = getFullUrl('/Crd/AddCredits', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default CrdAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    