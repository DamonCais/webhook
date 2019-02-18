using JVS_ADM.BasePage;
using JVS_ADM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ORM.SqlSugar.BLL;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
namespace JVS_ADM.ControllersApi
{
    public class PDController : MyApiController
    {
        pdMannager pd = new pdMannager();
        invManager ivn = new invManager();
        mbManager mb = new mbManager();
        [HttpGet]
        public HttpResponseMessage GetProductType(string ProjectCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C080_PDController_GetProductType";
            var msg = "";
            try
            {
                var list = pd.pd_ProductTypeSugar.GetList(p => p.ProjectCode == ProjectCode);
                response.SetContent(HttpStatus.ok, "获取成功", list, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetProductList(int ProductType, int pageIndex, int pageSize)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C081_PDController_GetProductList";
            var msg = "";
            try
            {
                var totalCount = 0;
                var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.pd.pd_ProductList>().Where(p => p.ProductType == ProductType).OrderBy(it => it.ProductCode).ToPageList(pageIndex, pageSize, ref totalCount);
                var rbj = new { list = list, totalCount = totalCount };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);



            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetProductDetail(string ProductCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C082_PDController_GetProductDetail";
            var msg = "";
            try
            {
                var model = pd.pd_ProductListSugar.GetById(ProductCode);
                response.SetContent(HttpStatus.ok, "获取成功", model, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpPost]
        public HttpResponseMessage AddRef()
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C083_PDController_AddRef";
            var msg = "";
            try
            {
                var productJson = context.Request["productJson"].ToString();
                var jo = JsonConvert.DeserializeObject(productJson) as JObject;
                var head = new ORM.SqlSugar.Model.inv.inv_RefHead();
                head.InOutStoreRef = "ref" + JVS_ADM.Common.Utils.GetRamCode();
                head.SupplierCode = jo["supplierCode"].ToString();
                head.TotalAmount = jo["totalAmount"].ToString().ToDecimal();
                head.InstoreTime = DateTime.Now;
                head.ShopID = jo["shopID"].ToString().ToInt32();
                head.EqmUID = jo["eqmUID"].ToString();
                head.StockMark = (short)jo["stockMark"].ToInt32();
                var pArry = jo["detail"] as JArray;
                var products = new List<ORM.SqlSugar.Model.pd.pd_ProductList>();
                var details = new List<ORM.SqlSugar.Model.inv.inv_RefList>();
                var stocks = new List<ORM.SqlSugar.Model.inv.inv_Stock>();
                foreach (var item in pArry)
                {

                    if (item["min"] != null && item["min"].ToString() != "0")
                    {
                        var productCode = "";
                        var min = Convert.ToInt32(item["min"]);
                        var refCode = item["refCode"].ToString();
                        var mcount = item["mcount"].ToInt32();
                        for (int i = 0; i < mcount; i++)
                        {
                            var reg = refCode.Substring(refCode.Length - min);
                            var code = refCode.Substring(0, refCode.Length - min);
                            var sg = (reg.ToInt32() + i).ToString();

                            if (sg.Length < min)
                            {
                                var zlength = min - sg.Length;
                                for (int j = 0; j < zlength; j++)
                                {
                                    sg = "0" + sg;
                                }
                            }
                            var product = new ORM.SqlSugar.Model.pd.pd_ProductList();
                            var detail = new ORM.SqlSugar.Model.inv.inv_RefList();
                            productCode = code + sg;
                            detail.InOutStoreRef = head.InOutStoreRef;
                            detail.Qty = item["qty"].ToString().ToInt32();
                            detail.UnitPrice = item["unitPrice"].ToString().ToDecimal();
                            detail.ActualAmount = item["actualAmount"].ToString().ToDecimal();
                            detail.ProductCode = productCode;
                            product = pd.pd_ProductListSugar.GetSingle(p => p.ProductCode == detail.ProductCode);
                            if (product == null)
                            {
                                product = new ORM.SqlSugar.Model.pd.pd_ProductList();
                                var ProductType = item["productType"].ToString().ToInt32();
                                var ProductName = item["productName"].ToString();
                                var IsLease = item["isLease"].ToString().ToBool();
                                product.ProductType = ProductType;
                                product.ProductName = ProductName;
                                product.IsLease = IsLease;
                                product.ProductCode = productCode;
                                product.UnitPrice = item["unitPrice"].ToString().ToDecimal();
                                product.Status = 0;
                                products.Add(product);
                            }
                            details.Add(detail);
                            var stock = new ORM.SqlSugar.Model.inv.inv_Stock();
                            stock.ShopID = head.ShopID;
                            stock.EqmUID = head.EqmUID;
                            stock.ProductCode = detail.ProductCode;
                            stock.AvgUnitPrice = detail.UnitPrice;
                            stock.Qty = detail.Qty;
                            stocks.Add(stock);
                        }

                    }
                    else
                    {
                        var product = new ORM.SqlSugar.Model.pd.pd_ProductList();
                        var detail = new ORM.SqlSugar.Model.inv.inv_RefList();
                        detail.InOutStoreRef = head.InOutStoreRef;
                        detail.Qty = item["qty"].ToString().ToInt32();
                        detail.UnitPrice = item["unitPrice"].ToString().ToDecimal();
                        detail.ActualAmount = item["actualAmount"].ToString().ToDecimal();
                        detail.ProductCode = item["productCode"].ToString();
                        product = pd.pd_ProductListSugar.GetSingle(p => p.ProductCode == detail.ProductCode);
                        if (product == null)
                        {
                            product = new ORM.SqlSugar.Model.pd.pd_ProductList();
                            var ProductType = item["productType"].ToString().ToInt32();
                            var ProductName = item["productName"].ToString();
                            var IsLease = item["isLease"].ToString().ToBool();
                            product.ProductType = ProductType;
                            product.ProductName = ProductName;
                            product.IsLease = IsLease;
                            product.ProductCode = item["productCode"].ToString();
                            product.UnitPrice = item["unitPrice"].ToString().ToDecimal();
                            product.Status = 0;
                            products.Add(product);
                        }
                        details.Add(detail);
                        var stock = new ORM.SqlSugar.Model.inv.inv_Stock();
                        stock.ShopID = head.ShopID;
                        stock.EqmUID = head.EqmUID;
                        stock.ProductCode = detail.ProductCode;
                        stock.AvgUnitPrice = detail.UnitPrice;
                        stock.Qty = detail.Qty;
                        stocks.Add(stock);
                    }





                }
                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    if (products.Count > 0)
                    {
                        pd.pd_ProductListSugar.InsertRange(products);
                    }
                    if (details.Count > 0)
                    {
                        ivn.inv_RefListSQLSugar.InsertRange(details);
                    }
                    if (stocks.Count > 0)
                    {
                        foreach (var item in stocks)
                        {
                            var stockModel = ivn.inv_StockSQLSugar.GetSingle(p => p.ProductCode == item.ProductCode && p.ShopID == item.ShopID && p.EqmUID == item.EqmUID);
                            if (stockModel != null)
                            {
                                stockModel.Qty += item.Qty;
                                ivn.inv_StockSQLSugar.Update(stockModel);
                            }
                            else
                            {
                                stockModel = new ORM.SqlSugar.Model.inv.inv_Stock();
                                stockModel.ProductCode = item.ProductCode;
                                stockModel.ShopID = item.ShopID;
                                stockModel.EqmUID = item.EqmUID;
                                stockModel.Qty = item.Qty;
                                stockModel.AvgUnitPrice = item.AvgUnitPrice;
                                ivn.inv_StockSQLSugar.Insert(stockModel);
                            }
                        }

                    }
                    ivn.inv_RefHeadSQLSugar.Insert(head);
                });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "操作成功", head, ResponseType.josn);
                }
                else
                {
                    AddErrorLog(LogEnum.pd, "操作失败", result.ErrorMessage);
                    response.SetContent(HttpStatus.error, "操作失败：" + result.ErrorMessage + "", head, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        //获取需要充卡的产品类型
        [HttpGet]
        public HttpResponseMessage GetCardType(string MemberCode, string ProjectCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C084_PDController_GetCardType";
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(ProjectCode))
                {
                    var CardType = pd.pd_ProductTypeSugar.GetList(p => p.SaleCard == true);
                    response.SetContent(HttpStatus.ok, "获取成功", CardType, ResponseType.josn);
                }
                else
                {
                    var CardType = pd.pd_ProductTypeSugar.GetList(p => p.SaleCard == true && p.ProjectCode == ProjectCode);
                    response.SetContent(HttpStatus.ok, "获取成功", CardType, ResponseType.josn);
                }
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage ProductCodeTrack(string MemberCode, string ProductCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C057_PDController_ProductCodeTrack";
            var msg = "";
            try
            {

                //var pmodel = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
                //var list = eqm.or.GetPageList(p => p.EqmUID == EqmUID, pmodel, p => p.CreateTime, OrderByType.Asc);
                //var rbj = new { list = list, totalCount = pmodel.PageCount };
                //response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                //return response;
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.pd, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
    }
}
