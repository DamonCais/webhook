using JVS_ADM.BasePage;
using ORM.SqlSugar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JVS_ADM.Common;
using IoT.Net.Manager;
using SqlSugar;

namespace JVS_ADM.ControllersApi
{
    public class EQMController : MyApiController
    {

        eqmManager eqm = new eqmManager();
        orgManager org = new orgManager();
        orderManager order = new orderManager();
        dictManager dict = new dictManager();
        mbManager mb = new mbManager();
        pdMannager pd = new pdMannager();
        [HttpGet]
        public HttpResponseMessage GetTopEQM(string MemberCode, string Latitude, string Longitude, int pageSize, int pageIndex)
        {


            var response = new MyHttpResponseMessage();
            response.apiNumber = "C040_EQMController_GetEQM";
            var msg = "";
            try
            {
                var da = eqm.GeteQm_EquipmentMySQL(Latitude, Longitude, pageSize, pageIndex, true, " where OnLine=1 ");
                var _total = eqm.eqm_EquipmentSugar.Count(p => p.OnLine == true);
                var rbj = new { data = da, total = _total };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetTopEQM(string MemberCode, string Latitude, string Longitude, int pageSize, int pageIndex, int OnLine, int ShopID, string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C042_EQMController_GetEQM";
            var msg = "";
            try
            {

                var is_OnLine = OnLine.ToBool();
                if (ShopID != -1 && string.IsNullOrEmpty(CorpCode))
                {
                    var da = eqm.GeteQm_EquipmentMySQL(Latitude, Longitude, pageSize, pageIndex, true, " where OnLine=" + OnLine + " and ShopID=" + ShopID + "");
                    var _total = eqm.eqm_EquipmentSugar.Count(p => p.OnLine == is_OnLine && p.ShopID == ShopID);
                    var rbj = new { data = da, total = _total };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }
                if (!string.IsNullOrEmpty(CorpCode) && ShopID == -1)
                {
                    var da = eqm.GeteQm_EquipmentMySQL(Latitude, Longitude, pageSize, pageIndex, true, " where OnLine=" + OnLine + " and CorpCode=" + CorpCode + "");
                    var _total = eqm.eqm_EquipmentSugar.Count(p => p.OnLine == is_OnLine && p.CorpCode == CorpCode);
                    var rbj = new { data = da, total = _total };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }
                if (string.IsNullOrEmpty(CorpCode) && ShopID == -1)// todo 改成与公司关联 代做
                {
                    var da = eqm.GeteQm_EquipmentMySQL(Latitude, Longitude, pageSize, pageIndex, true, " where OnLine=" + OnLine + " ");
                    var _total = eqm.eqm_EquipmentSugar.Count(p => p.OnLine == is_OnLine);
                    var rbj = new { data = da, total = _total };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetEQMDetail(string MemberCode, string EqmUID, int Source)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C043_EQMController_GetEQM";
            var msg = "";
            try
            {
                var equipmen = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                var shop = new ORM.SqlSugar.Model.org.org_Shop();
                if (equipmen != null)
                {
                    if (equipmen.ShopID != null)
                    {
                        shop = org.org_ShopSugar.GetById(equipmen.ShopID);
                    }
                }
                List<string> ProductCode = new List<string>();
                mbManager mb = new mbManager();
                var SModel = mb.mb_StockLSugar.GetList(p => p.MemberCode == MemberCode && p.Qty > 0);
                if (SModel.Any())
                {
                    ProductCode = SModel.Select(p => p.ProductCode).ToList();
                }


                var _switch = eqm.eqm_PartitionSwitchSugar.GetList(p => p.EqmUID == EqmUID).ToList();
                pdMannager pm = new pdMannager();
                var _switch2 = new List<object>();
                var reserve = order.order_ReservedSugar.GetList(p => p.EvaluateTime > DateTime.Now && p.Status == 1);
                foreach (var item in reserve)
                {
                    for (int i = 0; i < _switch.Count; i++)
                    {
                        if (item.ProductType == _switch[i].ProductType && _switch[i].Switch == 1 && item.MemberCode != MemberCode)
                        {
                            _switch[i].Switch = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < _switch.Count; i++)
                {
                    var sw = _switch[i];

                    if (Source == 2 && !string.IsNullOrEmpty(sw.ProductCode) && ProductCode.Count > 0 && !string.IsNullOrEmpty(ProductCode[0]))
                    {

                        var pModel = pm.pd_ProductListSugar.GetById(ProductCode[0]);
                        var typeModel = new ORM.SqlSugar.Model.pd.pd_ProductType();
                        if (pModel != null)
                        {
                            var pSwModel = pm.pd_ProductListSugar.GetById(sw.ProductCode);
                            if (pSwModel.ProductType != pModel.ProductType)
                            {
                                _switch[i].Switch = 0;
                            }
                            typeModel = pm.pd_ProductTypeSugar.GetById(pSwModel.ProductType);

                        }
                        _switch2.Add(new { sw = sw, pm = typeModel });
                    }
                    else
                    {
                        if (Source == 2 && ProductCode.Count <= 0)
                        {
                            _switch[i].Switch = 0;
                        }
                        _switch2.Add(new { sw = sw, pm = "" });
                    }


                }

                var rbj = new { equipmen = equipmen, shop = shop, _switch = _switch2, productCode = ProductCode };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage OpenEQM(string MemberCode, string EqmUID, string OldProductCode, decimal UnitPrice, decimal Qty, decimal PayAmount, decimal DiscountedAmt)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C045_EQMController_OpenEQM";
            var msg = "";
            try
            {
                var member = mb.mb_BaseInfoSQLSugar.GetById(MemberCode);
                if (member == null || member.ExpirationDate == null || member.ExpirationDate < DateTime.Now)
                {
                    response.SetContent(HttpStatus.error, "您还不是VIP会员！", "chongqian", ResponseType.josn);
                    return response;
                }

                var EqmU = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                if (EqmU == null)
                {
                    response.SetContent(HttpStatus.error, "设备不存在", "", ResponseType.josn);
                    return response;
                }
                var exit_model = order.order_HeaderSugar.GetSingle(p => p.EqmUID == EqmUID && p.Status != 3 && p.Status != 9 && p.OverdueTime > DateTime.Now);
                if (exit_model != null)
                {
                    response.SetContent(HttpStatus.error, "设备正在使用中，请稍后重试", exit_model.OverdueTime, ResponseType.josn);
                    return response;
                }

                var productOld = pd.pd_ProductListSugar.GetById(OldProductCode);
                if (productOld == null)
                {
                    response.SetContent(HttpStatus.error, "电池代码无效！", productOld, ResponseType.josn);
                    return response;
                }

                var project = dict.dict_ProjectListSQLSugar.GetById(EqmU.ProjectCode);
                if (project == null)
                {
                    response.SetContent(HttpStatus.error, "项目不存在！", project, ResponseType.josn);
                    return response;

                }
                if (productOld == null)
                {
                    response.SetContent(HttpStatus.error, "电池不存在！", productOld, ResponseType.josn);
                    return response;

                }
                var list = pd.pd_ProductTypeSugar.GetList(p => p.ProjectCode == "cabinet");
                var ProductType = pd.GetSaleCardType(list, productOld.ProductType.ToInt32());

                var vipCount = mb.mb_Assets.Count(p => p.MemberCode == MemberCode && p.ProductType == ProductType && p.ExpiryDate > DateTime.Now);
                if (vipCount <= 0)
                {
                    response.SetContent(HttpStatus.error, "您还没开通VIP会员或是会员已过期！", project, ResponseType.josn);
                    return response;
                }
                var count = eqm.eqm_PartitionSwitchSugar.Count(p => p.EqmUID == EqmUID && p.ProductType == productOld.ProductType && p.Switch == 1);
                if (count <= 0)
                {
                    response.SetContent(HttpStatus.error, "没有可用的单元，请稍后重试！", project, ResponseType.josn);
                    return response;
                }
                var or_head = new ORM.SqlSugar.Model.order.order_Header();
                var or_List = new ORM.SqlSugar.Model.order.order_List();
                var OrderRef = "Battery" + JVS_ADM.Common.Utils.GetRamCode();
                or_head.OrderRef = OrderRef;
                or_head.OrderTime = DateTime.Now;
                or_head.ProjectCode = EqmU.ProjectCode;
                or_head.MemberCode = MemberCode;
                or_head.TotalAmount = UnitPrice * Qty;
                or_head.PayAmount = PayAmount;
                or_head.DiscountedAmt = DiscountedAmt;
                or_head.mbAddressID = 0;
                or_head.OverdueTime = DateTime.Now.AddMinutes(1);
                or_head.Status = 0;
                or_head.ShopID = EqmU.ShopID;
                or_head.EqmUID = EqmU.EqmUID;
                or_head.Para1 = 0;
                or_head.Msg = productOld.ProductType.ToString() + "类型的电池";
                or_List.OrderRef = OrderRef;
                or_List.ProductCode = "";
                or_List.MemberCode = MemberCode;
                or_List.Qty = Qty;
                or_List.CreditsNo = 0;
                or_List.OldProductCode = OldProductCode;
                var Reservelist = order.order_ReservedSugar.GetList(p => p.MemberCode == MemberCode && p.EvaluateTime > DateTime.Now && p.Status == 1);


                var result = db.SqlServerClient.Ado.UseTran(() =>
                  {
                      var sysManager = new ORM.SqlSugar.BLL.sysManager();
                      var model = sysManager.AliConfigSQLSugar.GetById(1);
                      PubManager pum = new PubManager(model.AccessKey, model.SecretKey);
                      var rmsg = "";
                      Random rd = new Random();
                      var pupstr = "{\"method\":\"thing.service.property.set\",\"id\":\"222857295\",\"params\":{\"SwitchLast\":\"" + OrderRef + "_" + productOld.ProductType + "\"},\"version\":\"1.0.0\"}";
                      var presut = pum.Pub(project.ProductKey, EqmU.DeviceName, pupstr, out rmsg);
                      if (presut)
                      {
                          order.order_HeaderSugar.Insert(or_head);
                          order.order_ListSugar.Insert(or_List);
                          foreach (var item in Reservelist)
                          {
                              item.Status = 3;
                              order.order_ReservedSugar.Update(item);
                          }
                      }
                      else
                      {
                          response.SetContent(HttpStatus.error, "与硬件通讯失败", or_head, ResponseType.josn);
                      }


                  });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "操作成功", or_head, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, or_head, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage OpenMaintainEQM(string MemberCode, int Switch, string EqmUID, string MaintainProductCode, bool Discharge)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "A049_EQMController_OpenMaintainEQM";
            var msg = "";
            try
            {
                var EqmU = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                if (EqmU == null)
                {
                    response.SetContent(HttpStatus.error, "设备不存在", "", ResponseType.josn);
                    return response;
                }
                var exit_model = eqm.eqm_ExchangeListSugar.GetSingle(p => p.EqmUID == EqmUID && p.Status != 3 && p.Status != 9 && p.OverdueTime > DateTime.Now);
                if (exit_model != null)
                {
                    response.SetContent(HttpStatus.error, "设备正在使用中，请等待", exit_model.OverdueTime, ResponseType.josn);
                    return response;
                }
                var pd = new pdMannager();

                var project = dict.dict_ProjectListSQLSugar.GetById(EqmU.ProjectCode);
                if (project == null)
                {
                    response.SetContent(HttpStatus.error, "项目不存在！", project, ResponseType.josn);
                    return response;

                }
                var _switch = eqm.eqm_PartitionSwitchSugar.GetSingle(p => p.EqmUID == EqmUID && p.PartitionWallID == Switch);
                if (_switch == null)
                {
                    response.SetContent(HttpStatus.error, "该电池单元不可用！", project, ResponseType.josn);
                    return response;
                }

                if (!string.IsNullOrEmpty(MaintainProductCode))
                {
                    var count = eqm.eqm_PartitionSwitchSugar.Count(p => p.ProductCode == MaintainProductCode);
                    if (count > 0)
                    {
                        response.SetContent(HttpStatus.error, "该电池已在设备中！", project, ResponseType.josn);
                        return response;
                    }
                    var product = pd.pd_ProductListSugar.GetById(MaintainProductCode);
                    if (product == null)
                    {
                        response.SetContent(HttpStatus.error, "电池不存在！", product, ResponseType.josn);
                        return response;

                    }

                    if (product.ProductType != _switch.ProductType)
                    {
                        response.SetContent(HttpStatus.error, "该单元不能放这个电池！", project, ResponseType.josn);
                        return response;
                    }
                    var scount = mb.mb_StockLSugar.Count(p => p.Qty > 0 && p.ProductCode == MaintainProductCode);
                    if (scount > 0)
                    {
                        response.SetContent(HttpStatus.error, "这个电池已在会员使用中！", project, ResponseType.josn);
                        return response;
                    }
                }
                var or_head = new ORM.SqlSugar.Model.eqm.eqm_ExchangeList();
                var ExRef = "Exchange" + JVS_ADM.Common.Utils.GetRamCode();
                if (Discharge)
                {
                    ExRef = "Discharge" + JVS_ADM.Common.Utils.GetRamCode();
                }

                or_head.ExRef = ExRef;
                or_head.ExTime = DateTime.Now;
                or_head.MemberCode = MemberCode;
                or_head.OverdueTime = DateTime.Now.AddMinutes(1);
                or_head.Status = 0;
                or_head.ShopID = EqmU.ShopID;
                or_head.EqmUID = EqmU.EqmUID;
                or_head.ProductCode = MaintainProductCode;
                or_head.OldProductCode = "";
                or_head.Switch = Switch;
                var result = db.SqlServerClient.Ado.UseTran(() =>
            {
                var sysManager = new ORM.SqlSugar.BLL.sysManager();
                var model = sysManager.AliConfigSQLSugar.GetById(1);
                PubManager pum = new PubManager(model.AccessKey, model.SecretKey);
                var rmsg = "";
                Random rd = new Random();


                var pupstr = "{\"method\":\"thing.service.property.set\",\"id\":\"222940092\",\"params\":{\"MaintainLast\":\"" + ExRef + "_" + Switch + "_1\"},\"version\":\"1.0.0\"}";
                if (Discharge)
                {
                    pupstr = "{\"method\":\"thing.service.property.set\",\"id\":\"222940092\",\"params\":{\"Discharge\":\"" + ExRef + "_" + Switch + "_1\"},\"version\":\"1.0.0\"}";
                }
                var presut = pum.Pub(project.ProductKey, EqmU.DeviceName, pupstr, out rmsg);
                if (presut)
                {
                    eqm.eqm_ExchangeListSugar.Insert(or_head);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "与硬件通讯失败", or_head, ResponseType.josn);

                }


            });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "操作成功", or_head, ResponseType.josn);
                    return response;
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, or_head, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QeuryMaintainEQM(string ExRef)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C050_EQMController_QeuryMaintainEQM";
            var msg = "";
            try
            {
                var model = eqm.eqm_ExchangeListSugar.GetById(ExRef);
                if (model.Status == 3 || model.Status == 4)
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 3, ResponseType.josn);
                }
                else if (model.Status == 9)
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 9, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 0, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QeuryEQM(string OrderRef)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C046_EQMController_QeuryEQM";
            var msg = "";
            try
            {
                var model = order.order_HeaderSugar.GetById(OrderRef);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "订单已删除", 3, ResponseType.josn);
                    return response;
                }
                if (model.Status == 3 || model.Status == 4)
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 3, ResponseType.josn);
                }
                else if (model.Status == 9)
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 9, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.ok, "获取成功", 0, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage SetEQMOClose(string CloseOrderRef)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C048_EQMController_SetEQMOClose";
            var msg = "";
            try
            {
                var model = order.order_HeaderSugar.GetById(CloseOrderRef);
                if (model.Status == 3 || model.Status == 4)
                {
                    response.SetContent(HttpStatus.error, "已完成的订单不能关闭", 3, ResponseType.josn);
                    return response;
                }
                model.Status = 9;
                order.order_HeaderSugar.Update(model);
                response.SetContent(HttpStatus.ok, "关闭成功", 3, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QeuryEQMOrder(string MemberCode, string ProjectCode, int pageIndex, int pageSize, int Status)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C047_EQMController_QeuryEQMOrder";
            var msg = "";
            try
            {
                var totalCount = 0;
                var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.order.order_Header>().Where(p => p.MemberCode == MemberCode && p.ProjectCode == ProjectCode && p.Status == Status).OrderBy(it => it.OrderTime, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
                List<object> rbList = new List<object>();
                foreach (var item in list)
                {
                    var eqmModel = eqm.eqm_EquipmentSugar.GetById(item.EqmUID);
                    rbList.Add(new { item = item, eqm = eqmModel.EquipmentName });
                }
                var rbs = new { list = rbList, totalCount = totalCount };
                response.SetContent(HttpStatus.ok, "获取成功", rbs, ResponseType.josn);

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QeuryEQMOrderList(string ThOrderRef)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C048_EQMController_QeuryEQMOrderList";
            var msg = "";
            try
            {
                var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.order.order_List, ORM.SqlSugar.Model.pd.pd_ProductList>((st, sc) => new object[] {
        JoinType.Left,st.ProductCode==sc.ProductCode})
         .Select((st, sc) => new { st.OrderRef, st.MemberCode, st.ProductCode, st.Qty, st.UnitPrice, st.CreditsNo, st.OldProductCode, sc.ProductName }).Where(st => st.OrderRef == ThOrderRef).ToList();


                response.SetContent(HttpStatus.ok, "获取成功", list, ResponseType.josn);


            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage ChanageEQM(bool SetOnLine, string MemberCode, string EqmUID, string EquipmentName, string Services, string Address, double Longitude, double Latitude, string Logo)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "A050_EQMController_ChanageEQM";
            var msg = "";
            try
            {

                var model = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                model.OnLine = SetOnLine;
                if (model.OnLineTime == null)
                {
                    model.OnLineTime = DateTime.Now;
                }
                model.Logo = Logo;
                model.EquipmentName = EquipmentName;
                model.Services = Services;
                model.Address = Address;
                model.Longitude = Longitude;
                model.Latitude = Latitude; ;
                var result = eqm.eqm_EquipmentSugar.Update(model);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "操作成功", model, ResponseType.josn);
                    return response;
                }
                else
                {
                    response.SetContent(HttpStatus.error, "操作失败", model, ResponseType.josn);
                    return response;
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage TransferEQM(string MemberCode, string EqmUID, int ShopID)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C051_EQMController_TransferEQM";
            var msg = "";
            try
            {
                var eqmModel = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                if (eqmModel == null)
                {
                    response.SetContent(HttpStatus.error, "没有找到这个设备", eqmModel, ResponseType.josn);
                    return response;
                }
                if (eqmModel.ShopID == ShopID)
                {
                    response.SetContent(HttpStatus.error, "设备就属于当前营业部无需分配", eqmModel, ResponseType.josn);
                    return response;
                }
                var model = new ORM.SqlSugar.Model.eqm.eqm_Transfer();
                model.AssignBy = MemberCode;
                model.EqmUID = EqmUID;
                model.FromShopID = eqmModel.ShopID;
                model.ShopID = ShopID;
                model.TransTime = DateTime.Now;

                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    eqm.eqm_TransferSugar.Insert(model);
                    eqmModel.ShopID = ShopID;
                    eqm.eqm_EquipmentSugar.Update(eqmModel);
                });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "分配成功", model, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, null, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage CommandEQM(string EqmUID, int Command)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C052_EQMController_CommandEQM";
            var msg = "";
            try
            {
                var EqmU = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                if (EqmU == null)
                {
                    response.SetContent(HttpStatus.error, "设备不存在", "", ResponseType.josn);
                    return response;
                }
                var project = dict.dict_ProjectListSQLSugar.GetById(EqmU.ProjectCode);
                if (project == null)
                {
                    response.SetContent(HttpStatus.error, "项目不存在！", project, ResponseType.josn);
                    return response;

                }
                var sysManager = new ORM.SqlSugar.BLL.sysManager();
                var model = sysManager.AliConfigSQLSugar.GetById(1);
                PubManager pum = new PubManager(model.AccessKey, model.SecretKey);
                var rmsg = "";
                var pupstr = "{\"method\":\"thing.service.property.set\",\"id\":\"222857295\",\"params\":{\"Command\":\"" + Command + "_1\"},\"version\":\"1.0.0\"}";
                var presut = pum.Pub(project.ProductKey, EqmU.DeviceName, pupstr, out rmsg);
                if (presut)
                {

                    response.SetContent(HttpStatus.ok, "指令下发成功", "", ResponseType.josn);
                    return response;
                }
                else
                {
                    response.SetContent(HttpStatus.error, "与硬件通讯失败", rmsg, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage ReservedEQM(string MemberCode, string EqmUID, string OldProductCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C053_EQMController_ReservedEQM";
            var msg = "";
            try
            {
                var EqmU = eqm.eqm_EquipmentSugar.GetById(EqmUID);
                if (EqmU == null)
                {
                    response.SetContent(HttpStatus.error, "设备不存在", "", ResponseType.josn);
                    return response;
                }
                var productOld = pd.pd_ProductListSugar.GetById(OldProductCode);
                if (productOld == null)
                {
                    response.SetContent(HttpStatus.error, "您的电池代码无效！", productOld, ResponseType.josn);
                    return response;
                }

                var list = pd.pd_ProductTypeSugar.GetList(p => p.ProjectCode == "cabinet");
                var ProductType = pd.GetSaleCardType(list, productOld.ProductType.ToInt32());

                var vipCount = mb.mb_Assets.Count(p => p.MemberCode == MemberCode && p.ProductType == ProductType && p.ExpiryDate > DateTime.Now);
                if (vipCount <= 0)
                {
                    response.SetContent(HttpStatus.error, "您还没开通VIP会员或是会员已过期！", "", ResponseType.josn);
                    return response;
                }
                var count = eqm.eqm_PartitionSwitchSugar.Count(p => p.EqmUID == EqmUID && p.ProductType == productOld.ProductType) - order.order_ReservedSugar.Count(p => p.EvaluateTime > DateTime.Now && p.Status == 1);
                if (count <= 0)
                {
                    response.SetContent(HttpStatus.error, "没有可用的单元，请稍后重试！", "", ResponseType.josn);
                    return response;
                }
                var isExit = order.order_ReservedSugar.Count(p => p.MemberCode == MemberCode && p.EvaluateTime > DateTime.Now && p.Status == 1);
                if (isExit > 0)
                {
                    response.SetContent(HttpStatus.ok, "您已经有预约换电，不能重复预约", "", ResponseType.josn);
                    return response;
                }
                var model = new ORM.SqlSugar.Model.order.order_Reserved();
                model.Status = 1;
                model.ReservedRef = "Reserved" + JVS_ADM.Common.Utils.GetRamCode();
                model.MemberCode = MemberCode;
                model.ReservedTime = DateTime.Now;
                model.EvaluateTime = DateTime.Now.AddMinutes(30);
                model.ReservedTitle = "预约换电";
                model.EqmUID = EqmUID;
                model.ProductType = productOld.ProductType;
                var reuslt = order.order_ReservedSugar.Insert(model);
                if (reuslt)
                {
                    response.SetContent(HttpStatus.ok, "预约成功", model, ResponseType.josn);
                    return response;
                }
                else
                {
                    response.SetContent(HttpStatus.error, "预约失败", "", ResponseType.josn);
                    return response;
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage ReservedEQMList(string MemberCode, int pageIndex, int pageSize, int Status, bool isManager, string EqmUID)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C054_EQMController_ReservedEQMList";
            var msg = "";
            try
            {
                var exp = Expressionable.Create<ORM.SqlSugar.Model.order.order_Reserved>();
                exp.And(p => p.Status == Status);
                if (!isManager)
                {
                    exp.And(p => p.MemberCode == MemberCode);
                }
                if (string.IsNullOrEmpty(EqmUID))
                {
                    exp.And(p => p.EqmUID == EqmUID);
                }

                var totalCount = 0;
                var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.order.order_Reserved>().Where(exp.ToExpression()).OrderBy(it => it.ReservedTime).ToPageList(pageIndex, pageSize, ref totalCount);
                var rbj = new { list = list, totalCount = totalCount };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                return response;


            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QueryMyEQMReserveing(string MemberCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C055_EQMController_QueryMyEQMReserveing";
            var msg = "";
            try
            {
                var list = order.order_ReservedSugar.GetList(p => p.MemberCode == MemberCode && p.EvaluateTime > DateTime.Now && p.Status == 1);
                response.SetContent(HttpStatus.ok, "获取成功", list, ResponseType.josn);
                return response;

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage EditReservedEQM(string MemberCode, string ReservedRef, int Status)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C056_EQMController_EditReservedEQM";
            var msg = "";
            try
            {
                var model = order.order_ReservedSugar.GetById(ReservedRef);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "预约记录不存在", model, ResponseType.josn);
                    return response;
                }
                model.Status = Status;
                order.order_ReservedSugar.Update(model);
                response.SetContent(HttpStatus.ok, "修改成功", model, ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage PartitionErrorList(string MemberCode, string EqmUID, int PageIndex, int PageSize)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C057_EQMController_PartitionErrorList";
            var msg = "";
            try
            {

                var pmodel = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
                var list = eqm.eqm_PartitionErrorSugar.GetPageList(p => p.EqmUID == EqmUID, pmodel, p => p.CreateTime, OrderByType.Asc);
                var rbj = new { list = list, totalCount = pmodel.PageCount };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.eqm, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
       
    }
}