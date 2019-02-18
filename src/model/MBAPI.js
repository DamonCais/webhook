
    import { request } from "./request";
    class MBAPI{

    
            GetUserInfo(params) {
                let objList = ["Uid","UidType","RegCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetUserInfo', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            BindMobile(params) {
                let objList = ["UserCode","Mobile","PassWord","RegCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/BindMobile', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            BindUidType(params) {
                let objList = ["UserCode","Uid","UidType"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/BindUidType', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            ChanagePWD(params) {
                let objList = ["MemberCode","PassWord"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/ChanagePWD', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SaveMemberUser(params) {
                let objList = ["MemberCode","Avatar","RealName","IDCard","Sex","IDCardFrontImage","IDCardReverseImage","IDCardHoldImage","UrgentContactPerson","UrgentContactMobile","UrgentContactRelation"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/SaveMemberUser', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            BindProduct(params) {
                let objList = ["Mobile","ProductCode","ShopID","type","Qty","AvgUnitPrice"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/BindProduct', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryMBProduct(params) {
                let objList = ["QueryProductCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/QeuryMBProduct', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            MyMBProduct(params) {
                let objList = ["QueryMobile"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/MyMBProduct', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryMemberUser(params) {
                let objList = ["thMobile"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/QeuryMemberUser', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CardRecharge(params) {
                let objList = ["CardNo","MemberCode","PayWay"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/CardRecharge', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetMemberProdectTypeAndAssets(params) {
                let objList = ["MemberCode","ProjectCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetMemberProdectTypeAndAssets', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetMemberAssets(params) {
                let objList = ["MemberCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetMemberAssets', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetOrgCardList(params) {
                let objList = ["MemberCode","ProductType","pageIndex","pageSize","isManager","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetOrgCardList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetCorpMemberList(params) {
                let objList = ["MemberCode","pageIndex","pageSize","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetCorpMemberList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetMemberWallet(params) {
                let objList = ["MemberCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetMemberWallet', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SetMemberRole(params) {
                let objList = ["MemberCode","RoleID","ShopIDS"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/SetMemberRole', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CreatePowerCode(params) {
                let objList = ["PowerKey","PowerName","Description","NavID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/CreatePowerCode', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetPowerCodeList(params) {
                let objList = ["pageIndex","pageSize"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetPowerCodeList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            RoleAuthorization(params) {
                let objList = ["PowerKey","RoleIDS"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/RoleAuthorization', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CheckMyPower(params) {
                let objList = ["MemberCode","pageIndex","pageSize"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/CheckMyPower', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetRoles(params) {
                let objList = ["MemberCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/GetRoles', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CancelMBRoles(params) {
                let objList = ["MemberCode","RoleID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/CancelMBRoles', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            MberMessageList(params) {
                let objList = ["MemberCode","PageIndex","PageSize"];
                params=_.pick(params,objList);
                const href = getFullUrl('/MB/MberMessageList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default MBAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    