
    import { request } from "./request";
    class EQMAPI{

    
            GetTopEQM(params) {
                let objList = ["MemberCode","Latitude","Longitude","pageSize","pageIndex"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/GetTopEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetTopEQM(params) {
                let objList = ["MemberCode","Latitude","Longitude","pageSize","pageIndex","MemberCode","Latitude","Longitude","pageSize","pageIndex","OnLine","ShopID","CorpCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/GetTopEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            GetEQMDetail(params) {
                let objList = ["MemberCode","EqmUID","Source"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/GetEQMDetail', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            OpenEQM(params) {
                let objList = ["MemberCode","EqmUID","OldProductCode","UnitPrice","Qty","PayAmount","DiscountedAmt"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/OpenEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            OpenMaintainEQM(params) {
                let objList = ["MemberCode","Switch","EqmUID","MaintainProductCode","Discharge"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/OpenMaintainEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryMaintainEQM(params) {
                let objList = ["ExRef"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/QeuryMaintainEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryEQM(params) {
                let objList = ["OrderRef"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/QeuryEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            SetEQMOClose(params) {
                let objList = ["CloseOrderRef"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/SetEQMOClose', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryEQMOrder(params) {
                let objList = ["MemberCode","ProjectCode","pageIndex","pageSize","Status"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/QeuryEQMOrder', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QeuryEQMOrderList(params) {
                let objList = ["ThOrderRef"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/QeuryEQMOrderList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            ChanageEQM(params) {
                let objList = ["SetOnLine","MemberCode","EqmUID","EquipmentName","Services","Address","Longitude","Latitude","Logo"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/ChanageEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            TransferEQM(params) {
                let objList = ["MemberCode","EqmUID","ShopID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/TransferEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            CommandEQM(params) {
                let objList = ["EqmUID","Command"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/CommandEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            ReservedEQM(params) {
                let objList = ["MemberCode","EqmUID","OldProductCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/ReservedEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            ReservedEQMList(params) {
                let objList = ["MemberCode","pageIndex","pageSize","Status","isManager","EqmUID"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/ReservedEQMList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            QueryMyEQMReserveing(params) {
                let objList = ["MemberCode"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/QueryMyEQMReserveing', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            EditReservedEQM(params) {
                let objList = ["MemberCode","ReservedRef","Status"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/EditReservedEQM', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
            PartitionErrorList(params) {
                let objList = ["MemberCode","EqmUID","PageIndex","PageSize"];
                params=_.pick(params,objList);
                const href = getFullUrl('/EQM/PartitionErrorList', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            
    }
    export default EQMAPI;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    