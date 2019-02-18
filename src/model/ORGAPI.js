
    import { request } from "./request";
    class ORGAPI{

    
            GetOrgShop(params) {
                let objList = ["MemberCode","Latitude","Longitude","pageSize","pageIndex","OnLine","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetOrgShop', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCorpMaster(params) {
                let objList = ["MemberCode","pageSize","pageIndex","Status"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetCorpMaster', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCorpMasterByKeyWord(params) {
                let objList = ["KeyWord"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetCorpMasterByKeyWord', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetShopDetail(params) {
                let objList = ["ShopID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetShopDetail', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCorpDetail(params) {
                let objList = ["CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetCorpDetail', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            JoinInORG(params) {
                let objList = ["MemberCode","CorpCode","SocialCreditCode","CorpName","CorpProvince","CorpAddr","LicenseImage","ShopID","ShopName","ShopAddr","Remarks","Suggest","JoinType"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/JoinInORG', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetJoinIn(params) {
                let objList = ["MemberCode","CorpCode","pageSize","pageIndex","Status","isManager"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetJoinIn', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            VerifyJoinIn(params) {
                let objList = ["MemberCode","JoinTime"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/VerifyJoinIn', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetMyCorpAndShop(params) {
                let objList = ["CurrentMemberCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetMyCorpAndShop', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetShopOperate(params) {
                let objList = ["thisMemberCode","CorpCode","ShopID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetShopOperate', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetMealList(params) {
                let objList = ["MemberCode","CardCorpCode","OnLine","isAPP","ProductType"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetMealList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCardDetail(params) {
                let objList = ["MemberCode","CardNum"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetCardDetail', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CardCreate(params) {
                let objList = ["MemberCode","CardCorpCode","CardDescription","CardFaceValues","CardAmount","CardDuerationDay","CardOnLine","CardProductType","BundleCardNo1","BundleCardNo2","BundleCardNo3","BundleType","PictureURL","ShopIDS","FirstUse","CanBundle","BundleNumber1","BundleNumber2","BundleNumber3"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/CardCreate', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CardDelete(params) {
                let objList = ["MemberCode","CardNo"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/CardDelete', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CardRevise(params) {
                let objList = ["MemberCode","CardNo","CardDescription","CardFaceValues","CardAmount","CardDuerationDay","CardOnLine","CardProductType","BundleCardNo1","BundleCardNo2","BundleCardNo3","BundleType","PictureURL","FirstUse","CardBundleNumber1","CardBundleNumber2","CardBundleNumber3","CanBundle"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/CardRevise', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCanBundleCards(params) {
                let objList = ["MemberCode","CardCorpCodes"];
                params=_.pick(params,objList);
                const href = getFullUrl('/ORG/GetCanBundleCards', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default ORGAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    