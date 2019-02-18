using JVS_ADM.BasePage;
using ORM.SqlSugar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JVS_ADM.Common;
using SqlSugar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace JVS_ADM.ControllersApi
{
    public class ORGController : MyApiController
    {
        orderManager order = new orderManager();
        orgManager org = new orgManager();
        mbManager mb = new mbManager();
        pdMannager pd = new pdMannager();

        [HttpGet]
        public HttpResponseMessage GetOrgShop(string MemberCode, string Latitude, string Longitude, int pageSize, int pageIndex, int OnLine, string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C060_ORGController_GetOrgShop";
            var msg = "";
            try
            {
                var where = "where OnLine=" + OnLine + "";
                var _total = 0;
                if (!string.IsNullOrEmpty(CorpCode))
                {
                    where += " and CorpCode=" + CorpCode + "";
                    _total = org.org_ShopSugar.Count(p => p.OnLine == true && p.CorpCode == CorpCode);
                }
                else
                {
                    _total = org.org_ShopSugar.Count(p => p.OnLine == true);
                }
                var da = org.Getorg_ShopMySql( Latitude, Longitude, pageSize, pageIndex, true, "" + where + " ");

                var rbj = new { data = da, total = _total };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        public HttpResponseMessage GetCorpMaster(string MemberCode, int pageSize, int pageIndex, int Status)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C061_ORGController_GetCorpMaster";
            var msg = "";
            try
            {
                var da = org.Getorg_CorpMasterMySql( pageSize, pageIndex, " where Status=" + Status + " ");
                var _total = org.org_CorpMasterSugar.Count(p => p.Status == Status);
                var rbj = new { data = da, total = _total };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCorpMasterByKeyWord(string KeyWord)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C064_ORGController_GetCorpMasterByKeyWord";
            var msg = "";
            try
            {
                var da = org.Getorg_CorpMasterMySql( 100, 1, " where CorpName like '%" + KeyWord + "%' or CorpCode like '%" + KeyWord + "%'  ");

                response.SetContent(HttpStatus.ok, "获取成功", da, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetShopDetail(int ShopID)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C062_EQMController_GetShopDetail";
            var msg = "";
            try
            {
                var shop = org.org_ShopSugar.GetById(ShopID);
                var corp = new ORM.SqlSugar.Model.org.org_CorpMaster();
                if (shop != null)
                {
                    if (!string.IsNullOrEmpty(shop.CorpCode))
                    {
                        corp = org.org_CorpMasterSugar.GetById(shop.CorpCode);
                    }

                }
                var rbj = new { shop = shop, corp = corp };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCorpDetail(string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C063_EQMController_GetCorpDetail";
            var msg = "";
            try
            {
                var corp = org.org_CorpMasterSugar.GetById(CorpCode);

                response.SetContent(HttpStatus.ok, "获取成功", corp, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage JoinInORG(string MemberCode, string CorpCode, string SocialCreditCode, string CorpName, string CorpProvince, string CorpAddr, string LicenseImage, int ShopID, string ShopName, string ShopAddr, string Remarks, string Suggest, int JoinType)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C064_EQMController_JoinInORG";
            var msg = "";
            try
            {
                var JoinIn = new ORM.SqlSugar.Model.org.org_JoinIn();
                JoinIn.MemberCode = MemberCode;
                JoinIn.JoinType = JoinType;
                JoinIn.JoinTime = DateTime.Now;
                JoinIn.Remarks = Remarks;
                JoinIn.Suggest = Suggest;
                JoinIn.CorpCode = CorpCode;
                JoinIn.SocialCreditCode = SocialCreditCode;
                JoinIn.CorpName = CorpName;
                JoinIn.CorpProvince = CorpProvince;
                JoinIn.CorpAddr = CorpAddr;
                JoinIn.LicenseImage = LicenseImage;
                JoinIn.ShopID = ShopID;
                JoinIn.ShopName = ShopName;
                JoinIn.ShopAddr = ShopAddr;
                JoinIn.Status = 0;
                org.org_JoinInSugar.Insert(JoinIn);
                response.SetContent(HttpStatus.ok, "加盟成功", JoinIn, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetJoinIn(string MemberCode, string CorpCode, int pageSize, int pageIndex, int Status, bool isManager)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C065_ORGController_GetJoinIn";
            var msg = "";
            try
            {
                var totalCount = 0;
                var list = new List<ORM.SqlSugar.Model.org.org_JoinIn>();
                if (isManager)
                {
                    if (CorpCode == "000000")
                    {
                        list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.org.org_JoinIn>().Where(p => p.Status == Status).OrderBy(it => it.JoinTime).ToPageList(pageIndex, pageSize, ref totalCount);

                    }
                    else
                    {

                        list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.org.org_JoinIn>().Where(p => p.Status == Status && p.CorpCode == CorpCode).OrderBy(it => it.JoinTime).ToPageList(pageIndex, pageSize, ref totalCount);
                    }
                }
                else
                {
                    list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.org.org_JoinIn>().Where(p => p.Status == Status && p.MemberCode == MemberCode).OrderBy(it => it.JoinTime).ToPageList(pageIndex, pageSize, ref totalCount);
                }


                var rbj = new { data = list, total = totalCount };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage VerifyJoinIn(string MemberCode, string JoinTime)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C066_EQMController_VerifyJoinIn";
            var msg = "";
            try
            {
                var join = org.org_JoinInSugar.GetSingle(p => p.MemberCode == MemberCode && p.JoinTime == JoinTime.ToDateTime());
                if (join == null)
                {
                    response.SetContent(HttpStatus.error, "不存在这条记录", join, ResponseType.josn);
                    return response;
                }
                join.Status = 1;

                var result = db.SqlServerClient.Ado.UseTran(() =>
                  {


                      if (string.IsNullOrEmpty(join.CorpCode))
                      {
                          //创建企业
                          var corp = new ORM.SqlSugar.Model.org.org_CorpMaster();
                          var CorpCode = org.GetCorpCode(join.CorpProvince);
                          corp.CorpCode = CorpCode;
                          corp.CorpName = join.CorpName;
                          corp.SocialCreditCode = join.SocialCreditCode;
                          corp.RegisteredAddr = join.CorpAddr;
                          join.CorpCode = corp.CorpCode;
                          org.org_CorpMasterSugar.Insert(corp);
                      }
                      if (join.ShopID == 0)
                      {
                          //创建营业部
                          var shop = new ORM.SqlSugar.Model.org.org_Shop();
                          shop.ShopName = join.ShopName;
                          shop.Address = join.CorpAddr;
                          shop.CorpCode = join.CorpCode;
                          shop.DutyMemberCode = MemberCode;
                          var id = org.org_ShopSugar.InsertReturnIdentity(shop);
                          join.ShopID = id;
                      }
                      var mc = new ORM.SqlSugar.Model.mb.mb_MemberCorp();
                      mc.MemberCode = MemberCode;
                      mc.CorpCode = join.CorpCode;
                      mc.ShopID = join.ShopID.ToInt32();
                      mc.CreateTime = DateTime.Now;
                      mb.mb_MemberCorpSugar.Insert(mc);
                      org.org_JoinInSugar.Update(join);
                  });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "审核成功", join, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, null, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetMyCorpAndShop(string CurrentMemberCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C067_ORGController_GetMyCorpAndShop";
            var msg = "";
            try
            {
                var corps = mb.mb_MemberCorpSugar.GetList(p => p.MemberCode == CurrentMemberCode).Select(p => p.CorpCode);
                var list = org.org_CorpMasterSugar.GetList(p => corps.Contains(p.CorpCode));
                var rbjs = new List<object>();
                foreach (var item in list)
                {
                    var shops = org.org_ShopSugar.GetList(p => p.CorpCode == item.CorpCode);
                    var rbj = new { Corp = new { CorpCode = item.CorpCode, CorpName = item.CorpName, RegisteredAddr = item.RegisteredAddr }, shops };
                    rbjs.Add(rbj);
                }
                response.SetContent(HttpStatus.ok, "获取成功", rbjs, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetShopOperate(string thisMemberCode, string CorpCode, int ShopID)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C068_ORGController_GetShopOperate";
            var msg = "";
            try
            {

                if (ShopID == 0)
                {
                    var ids = org.org_ShopSugar.GetList(p => p.CorpCode == CorpCode).Select(p => p.ShopID).ToList();

                    var today = DateTime.Now.ToShortDateString().ToDateTime();
                    var monthDay = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-01").ToDateTime();
                    var todayCount = order.order_HeaderSugar.Count(p => ids.Contains((Int32)p.ShopID) && p.OrderTime > today);
                    var monthCount = order.order_HeaderSugar.Count(p => ids.Contains((Int32)p.ShopID) && p.OrderTime > monthDay);
                    var memberCount = mb.mb_BaseInfoSQLSugar.Count(p => p.NewestMobile != "");

                    var rbj = new { todayCount = todayCount, monthCount = monthCount, memberCount = memberCount, Amount = 0 };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }
                else
                {
                    var today = DateTime.Now.ToShortDateString().ToDateTime();
                    var monthDay = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-01").ToDateTime();
                    var todayCount = order.order_HeaderSugar.Count(p => p.ShopID == ShopID && p.OrderTime > today);
                    var monthCount = order.order_HeaderSugar.Count(p => p.ShopID == ShopID && p.OrderTime > monthDay);
                    var memberCount = mb.mb_BaseInfoSQLSugar.Count(p => p.NewestMobile != "");

                    var rbj = new { todayCount = todayCount, monthCount = monthCount, memberCount = memberCount, Amount = 0 };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetMealList(string MemberCode, string CardCorpCode, bool OnLine, bool isAPP, int ProductType)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C069_ORGController_GetMealList";
            var msg = "";
            try
            {

                var exp = Expressionable.Create<ORM.SqlSugar.Model.org.org_VirtualCard>();
                exp.And(p => p.OnLine == OnLine && p.CorpCode == CardCorpCode);
                var TypeResultList = new List<int>();
                var list1 = new List<object>();
                if (isAPP)
                {
                    if (ProductType != -1)
                    {

                        exp.And(p => p.ProductType == ProductType);
                    }

                }
                else
                {
                    var _types = mb.mb_Assets.GetList(p => p.MemberCode == MemberCode).Select(p => p.ProductType).ToList();
                    if (_types.Count > 0)
                    {
                        exp.And(p => (((_types.Contains(Convert.ToInt32(p.ProductType)) || p.ProductType == 0) && p.FirstUse == false || (!_types.Contains(Convert.ToInt32(p.ProductType)) || p.ProductType == 0) && p.FirstUse == true)));
                    }
                    else
                    {
                        exp.And(p => p.FirstUse == true);
                    }
                    if (ProductType != -1)
                    {
                        exp.And(p => p.ProductType == ProductType);
                    }

                }
                var ado = new ORM.SqlSugar.MySqlSugarClient();
                var list = ado.SqlServerClient.Queryable<ORM.SqlSugar.Model.org.org_VirtualCard>().Where(exp.ToExpression()).ToList();
                List<object> cards = new List<object>();

                foreach (var item in list)
                {
                    var name = pd.pd_ProductTypeSugar.GetList(p => p.ProductType == item.ProductType).Select(p => p.ProductTypeName).FirstOrDefault();
                    if (item.ProductType == 0)
                    {
                        name = "组合卡";
                    }
                    var CardNum1 = new ORM.SqlSugar.Model.org.org_VirtualCard();
                    var CardNum2 = new ORM.SqlSugar.Model.org.org_VirtualCard();
                    var CardNum3 = new ORM.SqlSugar.Model.org.org_VirtualCard();
                    if (item.BundleType.ToBool())
                    {
                        if (!string.IsNullOrEmpty(item.BundleCardNo1))
                        {
                            CardNum1 = org.org_VirtualCardSugar.GetById(item.BundleCardNo1); ;
                        }
                        if (!string.IsNullOrEmpty(item.BundleCardNo2))
                        {
                            CardNum2 = org.org_VirtualCardSugar.GetById(item.BundleCardNo2); ;
                        }
                        if (!string.IsNullOrEmpty(item.BundleCardNo3))
                        {
                            CardNum3 = org.org_VirtualCardSugar.GetById(item.BundleCardNo3); ;
                        }

                    }
                    else
                    {
                        CardNum3 = CardNum2 = CardNum1 = null;
                    }
                    var rbj = new { Card = item, ProductTypeName = name, CardNum1 = CardNum1, CardNum2 = CardNum2, CardNum3 = CardNum3 };
                    cards.Add(rbj);

                }
                response.SetContent(HttpStatus.ok, "获取成功", cards, ResponseType.josn);
                return response;
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCardDetail(string MemberCode, string CardNum)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C070_ORGController_GetCardDetail";
            var msg = "";
            try
            {
                var Card = org.org_VirtualCardSugar.GetById(CardNum);
                var list2 = pd.pd_ProductTypeSugar.GetList();
                var name = list2.Where(p => p.ProductType == Card.ProductType).Select(p => p.ProductTypeName).FirstOrDefault();
                var rbj = new
                {
                    CorpCode = Card.CorpCode,
                    CardNo = Card.CardNo,
                    Description = Card.Description,
                    FaceValue = Card.FaceValue,
                    Amount = Card.Amount,
                    DuerationDay = Card.DuerationDay,
                    OnLine = Card.OnLine,
                    ProductTypeName = name,
                    BundleCardNo1 = Card.BundleCardNo1,
                    BundleCardNo2 = Card.BundleCardNo2,
                    BundleCardNo3 = Card.BundleCardNo3,
                    BundleNumber1 = Card.BundleNumber1,
                    BundleNumber2 = Card.BundleNumber2,
                    BundleNumber3 = Card.BundleNumber3,
                    CanBundle = Card.CanBundle,
                    BundleType = Card.BundleType,
                    PictureURL = Card.PictureURL,
                    CreatedTime = Card.CreatedTime,
                    MemberCode = Card.MemberCode,
                    FirstUse = Card.FirstUse
                };
                response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage CardCreate(string MemberCode, string CardCorpCode, string CardDescription, float CardFaceValues, decimal CardAmount, int CardDuerationDay, bool CardOnLine, int CardProductType, string BundleCardNo1, string BundleCardNo2, string BundleCardNo3, bool BundleType, string PictureURL, string ShopIDS, bool FirstUse, bool CanBundle, int BundleNumber1, int BundleNumber2, int BundleNumber3)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C071_ORGController_CardCreate";
            var msg = "";
            try
            {
                var VirtualCard = new ORM.SqlSugar.Model.org.org_VirtualCard();
                var VirtualCardBelong = new ORM.SqlSugar.Model.org.org_VirtualCardBelong();

                VirtualCard.CorpCode = CardCorpCode;
                VirtualCard.Description = CardDescription;
                VirtualCard.FaceValue = CardFaceValues;
                VirtualCard.Amount = CardAmount;
                VirtualCard.OnLine = CardOnLine;
                VirtualCard.CanBundle = CanBundle;

                if (BundleType)
                {
                    if (BundleCardNo1 == null && BundleCardNo2 == null && BundleCardNo3 == null)
                    {
                        response.SetContent(HttpStatus.error, "组合卡至少绑定一张卡", BundleType, ResponseType.josn);
                        return response;
                    }

                    VirtualCard.BundleCardNo1 = BundleCardNo1;
                    VirtualCard.BundleCardNo2 = BundleCardNo2;
                    VirtualCard.BundleCardNo3 = BundleCardNo3;

                    VirtualCard.BundleNumber1 = BundleNumber1;
                    VirtualCard.BundleNumber2 = BundleNumber2;
                    VirtualCard.BundleNumber3 = BundleNumber3;

                    VirtualCard.DuerationDay = 0;
                    VirtualCard.ProductType = 0;
                    VirtualCard.CardNo = Utils.GetRamCode() + "_0";
                }
                else
                {
                    if (CardProductType == 0)
                    {
                        response.SetContent(HttpStatus.error, "非组合卡产品类型不能为0", CardProductType, ResponseType.josn);
                        return response;
                    }
                    VirtualCard.BundleCardNo1 = null;
                    VirtualCard.BundleCardNo2 = null;
                    VirtualCard.BundleCardNo3 = null;

                    VirtualCard.BundleNumber1 = 0;
                    VirtualCard.BundleNumber2 = 0;
                    VirtualCard.BundleNumber3 = 0;

                    VirtualCard.DuerationDay = CardDuerationDay;
                    VirtualCard.ProductType = CardProductType;
                    VirtualCard.CardNo = Utils.GetRamCode() + "_" + CardProductType;

                }

                VirtualCard.BundleType = BundleType;
                VirtualCard.PictureURL = PictureURL;
                VirtualCard.CreatedTime = DateTime.Now;
                VirtualCard.MemberCode = MemberCode;
                VirtualCard.FirstUse = FirstUse;
                var list = ShopIDS.ToArryList('_');// "1_2_3_4"

                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    foreach (var id in list)
                    {

                        VirtualCardBelong.ShopID = id.ToInt32();
                        VirtualCardBelong.CardNo = VirtualCard.CardNo;
                        org.org_VirtualCardBelongSugar.Insert(VirtualCardBelong);

                    }
                    org.org_VirtualCardSugar.Insert(VirtualCard);
                });

                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "创建成功", VirtualCard, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, result, ResponseType.josn);
                }
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage CardDelete(string MemberCode, string CardNo)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C072_ORGController_CardDelete";
            var msg = "";
            try
            {
                //var cardNo = org.org_VirtualCardSugar.GetById(CardNum);

                var result = db.SqlServerClient.Ado.UseTran(() =>
                {

                    org.org_VirtualCardSugar.DeleteById(CardNo);

                    org.org_VirtualCardBelongSugar.Delete(p => p.CardNo == CardNo);

                });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "删除成功", result, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "删除失败", result.ErrorMessage, ResponseType.josn);
                }
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage CardRevise(string MemberCode, string CardNo, string CardDescription, float CardFaceValues, decimal CardAmount, int CardDuerationDay, bool CardOnLine, int CardProductType, string BundleCardNo1, string BundleCardNo2, string BundleCardNo3, bool BundleType, string PictureURL, bool FirstUse, int CardBundleNumber1, int CardBundleNumber2, int CardBundleNumber3, bool CanBundle)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C073_ORGController_CardRevise";
            var msg = "";
            try
            {

                var VirtualCard = org.org_VirtualCardSugar.GetById(CardNo);

                VirtualCard.Description = CardDescription;
                VirtualCard.FaceValue = CardFaceValues;
                VirtualCard.Amount = CardAmount;
                VirtualCard.DuerationDay = CardDuerationDay;
                VirtualCard.OnLine = CardOnLine;
                VirtualCard.ProductType = CardProductType;
                VirtualCard.BundleCardNo1 = BundleCardNo1;
                VirtualCard.BundleCardNo2 = BundleCardNo2;
                VirtualCard.BundleCardNo3 = BundleCardNo3;
                VirtualCard.BundleNumber1 = CardBundleNumber1;
                VirtualCard.BundleNumber2 = CardBundleNumber2;
                VirtualCard.BundleNumber3 = CardBundleNumber3;
                VirtualCard.CanBundle = CanBundle;
                VirtualCard.BundleType = BundleType;
                VirtualCard.PictureURL = PictureURL;
                VirtualCard.FirstUse = FirstUse;
                var result = org.org_VirtualCardSugar.Update(VirtualCard);

                if (result)
                {
                    response.SetContent(HttpStatus.ok, "修改成功", result, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "修改失败", result, ResponseType.josn);
                }
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCanBundleCards(string MemberCode, string CardCorpCodes)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C074_ORGController_GetCanBundleCards";
            var msg = "";
            try
            {
                var Cards = org.org_VirtualCardSugar.GetList(p => p.CorpCode == CardCorpCodes && p.CanBundle == true);
                var list2 = pd.pd_ProductTypeSugar.GetList();
                var rbjs = new List<object>();
                foreach (var item in Cards)
                {

                    var name = list2.Where(p => p.ProductType == item.ProductType).Select(p => p.ProductTypeName).FirstOrDefault();
                    var rbj = new
                    {
                        CorpCode = item.CorpCode,
                        CardNo = item.CardNo,
                        Description = item.Description,
                        FaceValue = item.FaceValue,
                        Amount = item.Amount,
                        DuerationDay = item.DuerationDay,
                        OnLine = item.OnLine,
                        ProductTypeName = name,
                        BundleCardNo1 = item.BundleCardNo1,
                        BundleCardNo2 = item.BundleCardNo2,
                        BundleCardNo3 = item.BundleCardNo3,
                        BundleNumber1 = item.BundleNumber1,
                        BundleNumber2 = item.BundleNumber2,
                        BundleNumber3 = item.BundleNumber3,
                        CanBundle = item.CanBundle,
                        BundleType = item.BundleType,
                        PictureURL = item.PictureURL,
                        CreatedTime = item.CreatedTime,
                        MemberCode = item.MemberCode,
                        FirstUse = item.FirstUse
                    };
                    rbjs.Add(rbj);
                }
                response.SetContent(HttpStatus.ok, "获取成功", rbjs, ResponseType.josn);
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.org, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

    }
}
