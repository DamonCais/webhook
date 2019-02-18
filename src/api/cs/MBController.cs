using JVS_ADM.BasePage;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JVS_ADM.Common;
using ORM.SqlSugar.BLL;

namespace JVS_ADM.ControllersApi
{
    public class MBController : MyApiController
    {
        ORM.SqlSugar.BLL.mbManager mb = new ORM.SqlSugar.BLL.mbManager();
        orgManager org = new orgManager();
        pdMannager pd = new pdMannager();
        crdManager crd = new crdManager();

        [HttpGet]
        public HttpResponseMessage GetUserInfo(string Uid, string UidType, string RegCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C001_MBController_GetUserInfo";

            try
            {
                var model = new ORM.SqlSugar.Model.user.user_Infor();
                switch (UidType)
                {
                    case "Unionid":
                        model = mb.user_InforSQLSugar.GetSingle(p => p.Unionid == Uid);
                        if (model == null)
                        {
                            model = new ORM.SqlSugar.Model.user.user_Infor();
                            model.UserCode = Guid.NewGuid().ToString();
                            model.Unionid = Uid;
                            model.RegisterTime = DateTime.Now;
                            model.MemberCode = "";
                            model.Mobile = "";
                            mb.user_InforSQLSugar.Insert(model);
                        }
                        break;
                    case "OpenID":
                        model = mb.user_InforSQLSugar.GetSingle(p => p.OpenID == Uid);
                        if (model == null)
                        {
                            model = new ORM.SqlSugar.Model.user.user_Infor();
                            model.UserCode = Guid.NewGuid().ToString();
                            model.OpenID = Uid;
                            model.RegisterTime = DateTime.Now;
                            model.MemberCode = "";
                            model.Mobile = "";
                            mb.user_InforSQLSugar.Insert(model);
                        }
                        break;
                    case "APPID":
                        model = mb.user_InforSQLSugar.GetSingle(p => p.APPID == Uid);
                        if (model == null)
                        {
                            model = new ORM.SqlSugar.Model.user.user_Infor();
                            model.UserCode = Guid.NewGuid().ToString();
                            model.APPID = Uid;
                            model.RegisterTime = DateTime.Now;
                            model.MemberCode = "";
                            model.Mobile = "";
                            mb.user_InforSQLSugar.Insert(model);
                        }
                        break;
                    case "AliPayID":
                        model = mb.user_InforSQLSugar.GetSingle(p => p.AliPayID == Uid);
                        if (model == null)
                        {
                            model = new ORM.SqlSugar.Model.user.user_Infor();
                            model.UserCode = Guid.NewGuid().ToString();
                            model.AliPayID = Uid;
                            model.RegisterTime = DateTime.Now;
                            model.MemberCode = "";
                            model.Mobile = "";
                            mb.user_InforSQLSugar.Insert(model);
                        }

                        break;
                    case "MemberCode":
                        model = mb.user_InforSQLSugar.GetList(p => p.MemberCode == Uid).First();
                        if (model == null)
                        {
                            response.SetContent(HttpStatus.ok, "MemberCode无效", Uid, ResponseType.josn);
                            return response;
                        }

                        break;
                    case "PWD":
                        model = mb.user_InforSQLSugar.GetList(p => p.Mobile == Uid).First();
                        if (model == null)
                        {
                            response.SetContent(HttpStatus.ok, "Mobile无效", Uid, ResponseType.josn);
                            return response;
                        }
                        if (string.IsNullOrEmpty(model.MemberCode))
                        {
                            response.SetContent(HttpStatus.error, "该手机不是会员", Uid, ResponseType.josn);
                            return response;
                        }
                        var BaseInfo = mb.mb_BaseInfoSQLSugar.GetById(model.MemberCode);
                        if (BaseInfo == null)
                        {
                            response.SetContent(HttpStatus.error, "该手机不是有效会员", Uid, ResponseType.josn);
                            return response;
                        }
                        if (BaseInfo.PassWord != RegCode.MD5Encrypt(16))
                        {
                            response.SetContent(HttpStatus.error, "密码不正确", Uid, ResponseType.josn);
                            return response;
                        }
                        break;
                    case "Mobile":
                        model = mb.user_InforSQLSugar.GetSingle(p => p.Mobile == Uid);
                        if (!dicRegCode.Keys.Contains(Uid))
                        {
                            response.SetContent(HttpStatus.error, "验证码已过期", Uid, ResponseType.josn);
                            return response;
                        }
                        if (dicRegCode[Uid] != RegCode)
                        {
                            response.SetContent(HttpStatus.error, "验证码不正确", Uid, ResponseType.josn);
                            return response;
                        }
                        dicRegCode.Remove(Uid);
                        if (model == null)
                        {

                            model = new ORM.SqlSugar.Model.user.user_Infor();
                            model.UserCode = Guid.NewGuid().ToString();
                            model.RegisterTime = DateTime.Now;
                            model.MemberCode = "";
                            model.Mobile = Uid;
                            mb.user_InforSQLSugar.Insert(model);
                        }
                        break;

                    default:
                        break;
                }
                var member = new ORM.SqlSugar.Model.MemberInfor();
                member.user_Infor = model;
                member.IsMember = false;
                if (!string.IsNullOrEmpty(model.MemberCode))
                {
                    member.IsMember = true;
                    member.BaseInfo = mb.mb_BaseInfoSQLSugar.GetById(model.MemberCode);
                    var roles = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_Roles, ORM.SqlSugar.Model.mb.mb_MemberRole>((br, bm) => new object[] { JoinType.Right, br.RoleID == bm.RoleID && bm.MemberCode == model.MemberCode }).ToList();
                    member.Roles = roles;
                    if (roles != null && roles.Count > 0)
                    {
                        member.Powers = db.SqlServerClient.Ado.SqlQuery<ORM.SqlSugar.Model.mb.mb_Power>("select * from mb_Power where PowerKey in (select PowerKey from mb_MemberRole where RoleID in (" + roles.Select(p => p.RoleID).ToList().ToSealString(",") + ") ) ");
                    }
                    if (!string.IsNullOrEmpty(member.BaseInfo.Grade))
                    {
                        member.MemberGrade = mb.mb_MemberGradeSugar.GetById(member.BaseInfo.Grade);
                    }
                }
                var oline = mb.mb_OnLineSQLSugar.GetById(model.UserCode);
                if (oline == null)
                {
                    oline = new ORM.SqlSugar.Model.mb.mb_OnLine();
                    oline.LoginTime = DateTime.Now;
                    oline.MemberCode = model.MemberCode;
                    oline.ActivityTime = DateTime.Now;
                    oline.UserCode = model.UserCode;
                    oline.ExpiryTime = DateTime.Now.AddDays(7);
                    mb.mb_OnLineSQLSugar.Insert(oline);
                }
                else
                {
                    oline.ActivityTime = DateTime.Now;
                    oline.ExpiryTime = DateTime.Now.AddDays(7);
                    mb.mb_OnLineSQLSugar.Update(oline);
                }

                response.SetContent(HttpStatus.ok, "获取成功", member, ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {

                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }

        }
        [HttpGet]
        public HttpResponseMessage BindMobile(string UserCode, string Mobile, string PassWord, string RegCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C002_MBController_BindMobile";
            try
            {
                var model = new ORM.SqlSugar.Model.user.user_Infor();
                model = mb.user_InforSQLSugar.GetById(UserCode);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "用户不存在", model, ResponseType.josn);
                    return response;
                }
                var member = new ORM.SqlSugar.Model.mb.mb_BaseInfo();
                member = mb.mb_BaseInfoSQLSugar.GetSingle(p => p.NewestMobile == Mobile);
                bool exit = false;
                if (member != null)
                {
                    exit = true;
                    model.MemberCode = member.MemberCode;
                    mb.user_InforSQLSugar.Delete(p => p.UserCode != UserCode && p.Mobile == Mobile);
                }
                else
                {
                    member = new ORM.SqlSugar.Model.mb.mb_BaseInfo();
                }
                if (!dicRegCode.Keys.Contains(Mobile))
                {
                    response.SetContent(HttpStatus.error, "验证码已过期", Mobile, ResponseType.josn);
                    return response;
                }
                if (dicRegCode[Mobile] != RegCode)
                {
                    response.SetContent(HttpStatus.error, "验证码不正确", Mobile, ResponseType.josn);
                    return response;
                }
                dicRegCode.Remove(Mobile);
                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    if (exit)
                    {
                        mb.user_InforSQLSugar.Update(model);
                    }
                    else
                    {
                        var MemberCode = Guid.NewGuid().ToString();
                        model.Mobile = Mobile;
                        model.MemberCode = MemberCode;
                        mb.user_InforSQLSugar.Update(model);
                        member.MemberCode = MemberCode;
                        member.NewestMobile = Mobile;
                        member.Grade = "P";
                        member.PassWord = PassWord.MD5Encrypt(16);
                        member.CreateTime = DateTime.Now;
                        member.ExpirationDate = DateTime.Now;
                        mb.mb_BaseInfoSQLSugar.Insert(member);
                    }


                });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "绑定成功", member, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, member, ResponseType.josn);
                }

                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage BindUidType(string UserCode, string Uid, string UidType)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C003_MBController_BindUidType";
            try
            {
                var model = new ORM.SqlSugar.Model.user.user_Infor();
                model = mb.user_InforSQLSugar.GetById(UserCode);
                if (model == null || string.IsNullOrEmpty(model.MemberCode))
                {
                    response.SetContent(HttpStatus.error, "用户不存在！", model, ResponseType.josn);
                    return response;
                }
                int count = 0;
                switch (UidType)
                {
                    case "Unionid":

                        mb.user_InforSQLSugar.Delete(p => p.Unionid == Uid && p.UserCode != UserCode);
                        model.Unionid = Uid;
                        break;
                    case "OpenID":
                        mb.user_InforSQLSugar.Delete(p => p.OpenID == Uid && p.UserCode != UserCode);

                        model.OpenID = Uid;
                        break;
                    case "APPID":
                        mb.user_InforSQLSugar.Delete(p => p.APPID == Uid && p.UserCode != UserCode);

                        model.APPID = Uid;
                        break;
                    case "AliPayID":
                        mb.user_InforSQLSugar.Delete(p => p.AliPayID == Uid && p.UserCode != UserCode);

                        model.AliPayID = Uid;
                        break;
                    case "Mobile":
                        mb.user_InforSQLSugar.Delete(p => p.Mobile == Uid && p.UserCode != UserCode);

                        model.Mobile = Uid;
                        break;

                    default:
                        break;
                }


                var result = mb.user_InforSQLSugar.Update(model);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "绑定成功", model, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "绑定失败", model, ResponseType.josn);
                }

                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage ChanagePWD(string MemberCode, string PassWord)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C003_MBController_ChanagePWD";
            try
            {
                var model = new ORM.SqlSugar.Model.mb.mb_BaseInfo();
                model = mb.mb_BaseInfoSQLSugar.GetById(MemberCode);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "用户不存在", model, ResponseType.josn);
                    return response;
                }

                model.PassWord = PassWord.MD5Encrypt(16);
                var result = mb.mb_BaseInfoSQLSugar.Update(model);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "修改成功", model, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "修改失败", model, ResponseType.josn);
                }

                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage SaveMemberUser(string MemberCode, string Avatar, string RealName, string IDCard, int Sex, string IDCardFrontImage, string IDCardReverseImage, string IDCardHoldImage, string UrgentContactPerson, string UrgentContactMobile, string UrgentContactRelation)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C004_MBController_SaveMemberUser";
            try
            {
                var model = new ORM.SqlSugar.Model.mb.mb_BaseInfo();
                model = mb.mb_BaseInfoSQLSugar.GetById(MemberCode);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "用户不存在", model, ResponseType.josn);
                    return response;
                }
                model.Avatar = Avatar;
                model.RealName = RealName;
                model.IDCard = IDCard;
                model.Sex = (short)Sex;
                model.IDCardFrontImage = IDCardFrontImage;
                model.IDCardReverseImage = IDCardReverseImage;
                model.IDCardHoldImage = IDCardHoldImage;
                model.UrgentContactPerson = UrgentContactPerson;
                model.UrgentContactMobile = UrgentContactMobile;
                model.UrgentContactRelation = UrgentContactRelation;
                var result = mb.mb_BaseInfoSQLSugar.Update(model);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "修改成功", model, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "修改失败", model, ResponseType.josn);
                }

                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage BindProduct(string Mobile, string ProductCode, int ShopID, int type, decimal Qty, decimal AvgUnitPrice)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C005_MBController_BindProduct";
            try
            {

                var user = mb.mb_BaseInfoSQLSugar.GetSingle(p => p.NewestMobile == Mobile);
                if (user == null)
                {
                    response.SetContent(HttpStatus.error, "用户不存在", user, ResponseType.josn);
                    return response;
                }

                var pd = new pdMannager();
                var pro = pd.pd_ProductListSugar.GetById(ProductCode);
                if (pro == null)
                {
                    response.SetContent(HttpStatus.error, "产品不存在", user, ResponseType.josn);
                    return response;
                }
                if (type == 1)
                {
                    var count = mb.mb_StockLSugar.Count(p => p.ProductCode == ProductCode);
                    if (count > 0)
                    {
                        response.SetContent(HttpStatus.error, "已有其他用户拥有了这个产品", user, ResponseType.josn);
                        return response;
                    }
                    var stockModel = new ORM.SqlSugar.Model.mb.mb_Stock();
                    stockModel.ProductCode = ProductCode;
                    stockModel.MemberCode = user.MemberCode;
                    stockModel.ShopID = ShopID;
                    stockModel.EqmUID = "";
                    stockModel.Qty = Qty;
                    stockModel.AvgUnitPrice = AvgUnitPrice;
                    stockModel.LastInTime = DateTime.Now;
                    var result = mb.mb_StockLSugar.Insert(stockModel);
                    if (result)
                    {
                        user.ExpirationDate = "2019-04-30".ToDateTime();
                        mb.mb_BaseInfoSQLSugar.Update(user);
                        response.SetContent(HttpStatus.ok, "分配成功", user, ResponseType.josn);
                        return response;
                    }
                    response.SetContent(HttpStatus.error, "分配失败", user, ResponseType.josn);
                    return response;
                }
                if (type == 2)
                {
                    var result = mb.mb_StockLSugar.Delete(p => p.MemberCode == user.MemberCode && p.ProductCode == ProductCode);
                    if (result)
                    {
                        response.SetContent(HttpStatus.ok, "解绑成功", user, ResponseType.josn);
                        return response;
                    }
                    response.SetContent(HttpStatus.error, "解绑失败", user, ResponseType.josn);
                    return response;
                }


            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage QeuryMBProduct(string QueryProductCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C006_MBController_QeuryMBProduct";
            try
            {
                var pd = new pdMannager();
                var pros = mb.mb_StockLSugar.GetList(p => p.ProductCode == QueryProductCode);
                if (pros == null)
                {
                    response.SetContent(HttpStatus.error, "产品不存在", pros, ResponseType.josn);
                    return response;
                }
                List<object> list = new List<object>();
                foreach (var item in pros)
                {
                    var user = mb.mb_BaseInfoSQLSugar.GetById(item.MemberCode);
                    var mobile = "";
                    if (user != null)
                    {
                        mobile = user.NewestMobile;
                    }
                    list.Add(new { bj = item, mob = mobile });
                }
                response.SetContent(HttpStatus.ok, "查询成功", list, ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage MyMBProduct(string QueryMobile)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C007_MBController_QeuryMBProduct";
            try
            {
                mb.mb_StockLSugar.Delete(p => p.ProductCode == "");
                var pd = new pdMannager();
                var user = mb.mb_BaseInfoSQLSugar.GetList(p => p.NewestMobile == QueryMobile).First();

                if (user == null)
                {
                    response.SetContent(HttpStatus.error, "用户不存在", user, ResponseType.josn);
                    return response;
                }
                var pros = mb.mb_StockLSugar.GetList(p => p.MemberCode == user.MemberCode);
                response.SetContent(HttpStatus.ok, "查询成功", pros, ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage QeuryMemberUser(string thMobile)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C007_MBController_QeuryMBProduct";
            try
            {
                var pd = new pdMannager();
                var user = mb.mb_BaseInfoSQLSugar.GetList(p => p.NewestMobile == thMobile).First();

                if (user == null)
                {
                    response.SetContent(HttpStatus.error, "会员不存在", user, ResponseType.josn);
                    return response;
                }
                response.SetContent(HttpStatus.ok, "查询成功", user, ResponseType.josn);
                return response;

            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage CardRecharge(string CardNo, string MemberCode, Int32 PayWay)
        {
            bool PreToPay = false;
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C008_MBController_CardRecharge";
            try
            {
                // 待完善
                var Card = org.org_VirtualCardSugar.GetById(CardNo);
                if (Card == null)
                {
                    response.SetContent(HttpStatus.error, "没找到这个卡", "", ResponseType.josn);
                    return response;
                }
                if (Card.BundleType.ToBool())
                {

                    var kcount = mb.mb_Assets.Count(p => (p.ProductType == Card.BundleNumber1 || p.ProductType == Card.BundleNumber2 || p.ProductType == Card.BundleNumber3) && p.MemberCode == MemberCode);
                    if (kcount > 0 && Card.FirstUse.ToBool())
                    {
                        response.SetContent(HttpStatus.error, "您已经开户了，请购买续期套餐！", "", ResponseType.josn);
                        return response;
                    }
                    if (kcount <= 0 && !Card.FirstUse.ToBool())
                    {
                        response.SetContent(HttpStatus.error, "您还没有开户，请先购买开户卡！", "", ResponseType.josn);
                        return response;
                    }
                }
                else
                {
                    var kcount = mb.mb_Assets.Count(p => (p.ProductType == Card.ProductType) && p.MemberCode == MemberCode);
                    if (kcount > 0 && Card.FirstUse.ToBool())
                    {
                        response.SetContent(HttpStatus.error, "您已经开户了，请购买续期套餐！", "", ResponseType.josn);
                        return response;
                    }
                    if (kcount <= 0 && !Card.FirstUse.ToBool())
                    {
                        response.SetContent(HttpStatus.error, "您还没有开户，请先购买开户卡！", "", ResponseType.josn);
                        return response;
                    }
                }



                var RechargeRecord = new ORM.SqlSugar.Model.mb.mb_CardList();

                RechargeRecord.MemberCode = MemberCode;
                RechargeRecord.CardNo = "Card" + CardNo + "_" + mb.mb_CardList.Count(p => 1 == 1) + "";
                RechargeRecord.FaceValue = Card.FaceValue.ToDecimal();
                RechargeRecord.DuerationDay = Card.DuerationDay;
                RechargeRecord.Amount = Card.Amount;
                RechargeRecord.GotTime = DateTime.Now;
                RechargeRecord.ProductType = Card.ProductType;
                RechargeRecord.Status = 0;
                RechargeRecord.PayWay = PayWay;
                RechargeRecord.PayNo = "";
                RechargeRecord.VCardNo = CardNo;

                var result = db.SqlServerClient.Ado.UseTran(() =>
                {

                    mb.mb_CardList.Insert(RechargeRecord);
                });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "订单生成成功", RechargeRecord, ResponseType.josn);
                    return response;
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, "", ResponseType.josn);
                    return response;
                }


            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetMemberProdectTypeAndAssets(string MemberCode, string ProjectCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C011_MBController_GetMemberProdectTypeAndAssets";
            try
            {
                var ptyCards = pd.pd_ProductTypeSugar.GetList(p => p.IsLease == true && p.ProjectCode == ProjectCode && p.SaleCard == true);
                List<object> list = new List<object>();
                foreach (var item in ptyCards)
                {
                    var mbAsset = mb.mb_Assets.GetSingle(p => p.MemberCode == MemberCode && p.ProductType == item.ProductType);
                    var proType = pd.pd_ProductTypeSugar.GetById(item.ProductType);
                    var rbj = new { ProductType = item, mbAsset = mbAsset };
                    list.Add(rbj);

                }

                response.SetContent(HttpStatus.ok, "查询成功", list, ResponseType.josn);
                return response;

            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMemberAssets(string MemberCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C009_MBController_GetMemberAssets";
            try
            {

                var ProductCodes = mb.mb_StockLSugar.GetList(p => p.MemberCode == MemberCode).Select(p => p.ProductCode);
                List<object> list = new List<object>();
                var pros = pd.pd_ProductListSugar.GetList(p => ProductCodes.Contains(p.ProductCode));
                var mbAssets = mb.mb_Assets.GetList(p => p.MemberCode == MemberCode).ToList();
                foreach (var item in mbAssets)
                {
                    var proType = pd.pd_ProductTypeSugar.GetById(item.ProductType);
                    if (proType == null)
                    {
                        continue;
                    }
                    var prds = new List<ORM.SqlSugar.Model.pd.pd_ProductList>();
                    var Types = pd.pd_ProductTypeSugar.GetList(p => p.ProjectCode == proType.ProjectCode);
                    foreach (var item2 in pros)
                    {
                        var saleType = pd.GetSaleCardType(Types, item2.ProductType.ToInt32());
                        if (saleType == item.ProductType)
                        {
                            prds.Add(item2);

                        }
                    }
                    var rbj = new { pro = prds, ProductType = proType, assets = item };
                    list.Add(rbj);



                }

                response.SetContent(HttpStatus.ok, "查询成功", list, ResponseType.josn);
                return response;

            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgCardList(string MemberCode, int ProductType, int pageIndex, int pageSize, bool isManager, string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C012_MBController_GetOrgCardList";
            var msg = "";
            try
            {
                if (isManager)
                {

                    var totalCount = 0;
                    var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_CardList, ORM.SqlSugar.Model.mb.mb_MemberCorp>((c, m) => new object[] { JoinType.Right, c.MemberCode == m.MemberCode }).Where((c, m) => m.CorpCode == CorpCode && c.ProductType == ProductType && c.Status == 3).Select((c, m) => new { c, m }).ToPageList(pageIndex, pageSize, ref totalCount);
                    List<object> bjlist = new List<object>();

                    foreach (var item in list)
                    {
                        if (item.c != null && item.c.VCardNo != null)
                        {

                            var vc = org.org_VirtualCardSugar.GetById(item.c.VCardNo.ToString());
                            var ibj = new { item = item, vc = vc };
                            bjlist.Add(ibj);
                        }


                    }
                    var rbj = new { list = bjlist, totalCount = totalCount };

                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                }
                else
                {
                    var totalCount = 0;
                    var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_CardList>().Where(p => p.ProductType == ProductType && p.MemberCode == MemberCode && p.Status == 3).OrderBy(it => it.GotTime).ToPageList(pageIndex, pageSize, ref totalCount);
                    List<object> bjlist = new List<object>();
                    foreach (var item in list)
                    {
                        var vc = org.org_VirtualCardSugar.GetById(item.VCardNo.ToString());
                        var ibj = new { item = item, vc = vc };
                        bjlist.Add(ibj);

                    }
                    var rbj = new { list = bjlist, totalCount = totalCount };

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
        public HttpResponseMessage GetCorpMemberList(string MemberCode, int pageIndex, int pageSize, string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C013_MBController_GetCorpMemberList";
            var msg = "";
            try
            {


                var totalCount = 0;
                var list = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_BaseInfo, ORM.SqlSugar.Model.mb.mb_MemberCorp>((c, m) => new object[] { JoinType.Right, c.MemberCode == m.MemberCode }).Where((c, m) => m.CorpCode == CorpCode).Select((c, m) => new { c, m }).ToPageList(pageIndex, pageSize, ref totalCount);
                var rbj = new { list = list, totalCount = totalCount };
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
        //获取钱包信息
        [HttpGet]
        public HttpResponseMessage GetMemberWallet(string MemberCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C014_MBController_GetMemberWallet";
            var msg = "";
            try
            {
                var list = mb.mb_WalletSugar.GetList(p => p.MemberCode == MemberCode);
                if (list.Count == 0)
                {
                    var mbWallet = new ORM.SqlSugar.Model.mb.mb_Wallet();
                    mbWallet.MemberCode = MemberCode;
                    mbWallet.Balance = 0;
                    mbWallet.Credits = 0;
                    mbWallet.Currency = "RMB";
                    var result = mb.mb_WalletSugar.Insert(mbWallet);
                    if (result)
                    {

                        response.SetContent(HttpStatus.ok, "获取成功", mbWallet, ResponseType.josn);
                    }
                    else
                    {
                        response.SetContent(HttpStatus.error, "获取失败", mbWallet, ResponseType.josn);
                    }

                }
                else
                {
                    response.SetContent(HttpStatus.ok, "获取成功", list, ResponseType.josn);

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
        public HttpResponseMessage SetMemberRole(string MemberCode, int RoleID, string ShopIDS)
        {

            var response = new MyHttpResponseMessage();
            response.apiNumber = "C015_MBController_SetMemberRole";
            var msg = "";
            try
            {
                var ShopIDSList = ShopIDS.ToArryList('_');
                if (ShopIDSList.Count == 0)
                {
                    response.SetContent(HttpStatus.error, "营业部不能为空", ShopIDSList.Count, ResponseType.josn);
                    return response;
                }

                var role = new ORM.SqlSugar.Model.mb.mb_MemberRole();
                role.MemberCode = MemberCode;
                role.RoleID = RoleID.ToInt32();
                var result = db.SqlServerClient.Ado.UseTran(() =>
                 {
                     mb.mb_MemberRoleSugar.Insert(role);

                     foreach (var item in ShopIDSList)
                     {
                         var memberCrop = new ORM.SqlSugar.Model.mb.mb_MemberCorp();
                         memberCrop.MemberCode = MemberCode;
                         memberCrop.CorpCode = org.org_ShopSugar.GetById(item.ToInt32()).CorpCode;
                         memberCrop.ShopID = item.ToInt32();
                         memberCrop.CreateTime = DateTime.Now;
                         mb.mb_MemberCorpSugar.Insert(memberCrop);
                     }

                 });
                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "分配成功", result, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "分配失败", result.ErrorMessage, ResponseType.josn);
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
        public HttpResponseMessage CreatePowerCode(string PowerKey, string PowerName, string Description, int NavID)
        {
            //NavID是什么？
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C016_MBController_CreatePowerCode";
            var msg = "";
            try
            {
                var power = new ORM.SqlSugar.Model.mb.mb_Power();
                power.PowerKey = PowerKey;
                power.PowerName = PowerName;
                power.Description = Description;
                power.NavID = NavID;

                var result = mb.mb_PowerSugar.Insert(power);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "增加成功", power, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "增加失败", power, ResponseType.josn);
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
        public HttpResponseMessage GetPowerCodeList(int pageIndex, int pageSize)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C017_MBController_GetPowerCodeList";
            var msg = "";
            var totalCount = 0;
            try
            {
                //var Powers = mb.mb_PowerSugar.GetList();
                var Powers = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_Power>().ToPageList(pageIndex, pageSize, ref totalCount);
                //var power = new ORM.SqlSugar.Model.mb.mb_MemberRole();
                var powerList = new List<object>();
                foreach (var item in Powers)
                {
                    var power = new
                    {
                        PowerKey = item.PowerKey,
                        PowerName = item.PowerName,
                        Description = item.Description,
                        NavID = item.NavID
                    };

                    powerList.Add(power);
                }
                var resultList = new { powerList, totalCount = totalCount };
                bool result = powerList != null;
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "查询成功", resultList, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.ok, "权限列表中没有数据", powerList, ResponseType.josn);
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
        public HttpResponseMessage RoleAuthorization(string PowerKey, string RoleIDS)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C018_MBController_RoleAuthorization";
            var msg = "";
            try
            {
                var roleIDList = RoleIDS.ToArryList('_');
                foreach (var RoleID in roleIDList)
                {
                    var rolePower = new ORM.SqlSugar.Model.mb.mb_RolePower();
                    rolePower.PowerKey = PowerKey;
                    rolePower.RoleID = RoleID.ToInt32();

                    bool result = mb.mb_RolePowerSugar.Insert(rolePower);
                    if (result)
                    {
                        response.SetContent(HttpStatus.ok, "授权成功", result, ResponseType.josn);
                    }
                    else
                    {
                        response.SetContent(HttpStatus.error, "授权失败", result, ResponseType.josn);
                    }
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
        public HttpResponseMessage CheckMyPower(string MemberCode, int pageIndex, int pageSize)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C019_MBController_CheckMyPower";
            var msg = "";
            try
            {
                var totalCount = 0;
                var rbjs = new List<object>();
                var powerNamelist = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_MemberRole, ORM.SqlSugar.Model.mb.mb_RolePower, ORM.SqlSugar.Model.mb.mb_Power>((mr, rp, p) => new object[] { JoinType.Left, mr.RoleID == rp.RoleID, JoinType.Left, rp.PowerKey == p.PowerKey }).Where(mr => mr.MemberCode == MemberCode).Select((mr, rp, p) => new { p.PowerName, p.Description, p.PowerKey }).ToPageList(pageIndex, pageSize, ref totalCount);
                foreach (var item in powerNamelist)
                {
                    var rbj = new { MemberCode = MemberCode, PowerKey = item.PowerKey, PowerName = item.PowerName, Description = item.Description };
                    rbjs.Add(rbj);
                }
                var resultList = new { Powers = rbjs, totalCount = totalCount };
                response.SetContent(HttpStatus.ok, "查询成功", resultList, ResponseType.josn);

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
        public HttpResponseMessage GetRoles(string MemberCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C020_MBController_GetRoles";
            var msg = "";
            try
            {
                var mbRoles = mb.mb_MemberRoleSugar.GetList(p => p.MemberCode == MemberCode);

                if (mbRoles.Exists(p => p.RoleID == 1))
                {
                    var result = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_Roles>().ToList();

                    response.SetContent(HttpStatus.ok, "获取成功", result, ResponseType.josn);
                    return response;
                }

                var Roles = mbRoles.Select(p => p.RoleID).ToList();
                var UpRoleIDS = new List<int>();

                foreach (var item in Roles)
                {
                    var UpRoleIDList = mb.mb_RolesSQLSugar.GetSingle(p => p.RoleID == item);

                    UpRoleIDS.Add(UpRoleIDList.UpRoleID.ToInt32());
                }
                if (UpRoleIDS.Exists(p => p == 0) && UpRoleIDS.Count == 1)
                {
                    var result1 = mb.mb_RolesSQLSugar.GetList().Where(p => p.RoleID != 1 && p.UpRoleID == 0);
                    response.SetContent(HttpStatus.ok, "获取成功", result1, ResponseType.josn);
                }
                else
                {
                    UpRoleIDS.Remove(0);
                    int maxPowerID = UpRoleIDS.Min();

                    var result2 = mb.mb_RolesSQLSugar.GetList().Where(p => p.UpRoleID >= maxPowerID).ToList();
                    result2.Add(mb.mb_RolesSQLSugar.GetSingle(p => p.RoleID != 1 && p.UpRoleID == 0));
                    response.SetContent(HttpStatus.ok, "获取成功", result2, ResponseType.josn);
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
        public HttpResponseMessage CancelMBRoles(string MemberCode, int RoleID)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C021_MBController_CancelMBRoles";
            var msg = "";
            try
            {
                //var shopIDSList = ShopIDS.ToArryList('_');
                var mbRole = new ORM.SqlSugar.Model.mb.mb_MemberRole();
                mbRole.RoleID = RoleID;
                mbRole.MemberCode = MemberCode;
                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    mb.mb_MemberRoleSugar.Delete(mbRole);

                });
                if (result.IsSuccess)
                {

                    response.SetContent(HttpStatus.ok, "取消授权成功", result, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "失败", result.ErrorMessage, ResponseType.josn);
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
        public HttpResponseMessage MberMessageList(string MemberCode, int PageIndex, int PageSize)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C022_MBController_MberMessageList";
            var msg = "";
            try
            {
                var pmodel = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
                var list = mb.MessageSugar.GetPageList(p => p.MemberCode == MemberCode, pmodel, p => p.MessageTitme, OrderByType.Asc);
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
