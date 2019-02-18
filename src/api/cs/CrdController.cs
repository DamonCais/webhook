using JVS_ADM.BasePage;
using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ORM.SqlSugar.BLL;
using JVS_ADM.Common;


namespace JVS_ADM.ControllersApi
{
    public class CrdController : MyApiController
    {
        crdManager crd = new crdManager();
        mbManager mb = new mbManager();



        [HttpGet]
        public HttpResponseMessage BalanceRecharge(int BalanceID, string MemberCode,int PayWay)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C100_CrdController_BalanceRecharge";
            var msg = "";
            try
            {
                var balanceType = crd.crd_BalanceTypeSQLSugar.GetSingle(p => p.BalanceID == BalanceID&&p.IsOnline==true);

                if (balanceType == null)
                {
                    response.SetContent(HttpStatus.error, "未能找到相关充值类型", balanceType, ResponseType.josn);
                    return response;
                }

                var crdLog = new ORM.SqlSugar.Model.crd.crd_BalanceLog();

                crdLog.BalanceNo = "Balance" + Utils.GetRamCode() + "";
                crdLog.Balance = balanceType.OriginalAmount + balanceType.DonationAmount;
                crdLog.OriginalBalance = balanceType.OriginalAmount;
                crdLog.DonationBalance = balanceType.DonationAmount; 
                crdLog.BalanceID = BalanceID;
                crdLog.MemberCode = MemberCode;
                crdLog.BalanceType = 1;
                crdLog.Remark = "";
                crdLog.TradeNo = "";
                crdLog.PayTime = null;
                crdLog.PayWay = PayWay;
                crdLog.Status = 0;
                crdLog.CreateDate = DateTime.Now;
                var result = db.SqlServerClient.Ado.UseTran(() =>
                {
                    crd.crd_BalanceLogSQLSugar.Insert(crdLog);
                });

                if (result.IsSuccess)
                {
                    response.SetContent(HttpStatus.ok, "订单生成成功", crdLog, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, result.ErrorMessage, "", ResponseType.josn);
                }
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage BalanceTypeCreate(int BalanceID,string Description,decimal OriginalAmount,decimal DonationAmount,bool IsOnline,string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C101_CrdController_BalanceTypeCreate";
            var msg = "";
            try
            {
                var BalanceType =new ORM.SqlSugar.Model.crd.crd_BalanceType();

                BalanceType.BalanceID = BalanceID;
                BalanceType.Description = Description;
                BalanceType.OriginalAmount = OriginalAmount;
                BalanceType.DonationAmount = DonationAmount;
                BalanceType.IsOnline = IsOnline;
                BalanceType.CreateTime = DateTime.Now;
                BalanceType.CorpCode = CorpCode;

                var result = crd.crd_BalanceTypeSQLSugar.Insert(BalanceType);
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "插入成功", BalanceType, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "插入失败", "", ResponseType.josn);

                }
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetBalanceTypeList(bool IsOnline)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C102_CrdController_GetBalanceTypeList";
            var msg = "";
            try
            {
                var BalanceType = crd.crd_BalanceTypeSQLSugar.GetList(p=>p.IsOnline==true);
                var lists = new List<object>();
                foreach (var item in BalanceType)
                {
                    var list = new { BalanceID = item.BalanceID, CreateTime = item.CreateTime, Description = item.Description, IsOnline = item.IsOnline, OriginalAmount = item.OriginalAmount, DonationAmount = item.DonationAmount };
                    lists.Add(list);
                }
                response.SetContent(HttpStatus.ok, "查询成功", lists, ResponseType.josn);
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage SetBalanceType(int BalanceID,string Description, decimal OriginalAmount, decimal DonationAmount, bool IsOnline,string CorpCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C103_CrdController_SetBalanceType";
            var msg = "";
            try
            {
                var BalanceType = crd.crd_BalanceTypeSQLSugar.GetById(BalanceID);
                BalanceType.Description = Description;
                BalanceType.OriginalAmount = OriginalAmount;
                BalanceType.DonationAmount = DonationAmount;
                BalanceType.IsOnline = IsOnline;
                BalanceType.CorpCode = CorpCode;

                var result = crd.crd_BalanceTypeSQLSugar.Insert(BalanceType);

                response.SetContent(HttpStatus.ok, "修改成功", result, ResponseType.josn);
            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

        //[HttpGet]
        //public HttpResponseMessage AddCredits(string MemberCode,short CreditsID,string Remark)
        //{
        //    var response = new MyHttpResponseMessage();
        //    response.apiNumber = "C104_CrdController_AddCredits";
        //    var msg = "";
        //    try
        //    {
        //        var CreditsGot = new ORM.SqlSugar.Model.crd.crd_CreditsGot();
        //        var creditsType = crd.crd_CreditsTypeSQLSugar.GetById(CreditsID);
        //        var mbWallet = mb.mb_WalletSugar.GetById(MemberCode);
        //        var mbGrade = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.mb.mb_MemberGrade>().OrderBy(p => p.GradeSeq, OrderByType.Asc).ToList();
        //        var mbBaseInfo = mb.mb_BaseInfoSQLSugar.GetById(MemberCode);
        //        CreditsGot.CreditsNo= "Credits" + Utils.GetRamCode() + "_" + crd.crd_CreditsTypeSQLSugar.Count(p => 1 == 1) + "";
        //        CreditsGot.CreditsID = CreditsID;
        //        CreditsGot.MemberCode = MemberCode;
        //        CreditsGot.CreditsTime = DateTime.Now;
        //        CreditsGot.Credits = creditsType.Credits;
        //        CreditsGot.Remark = Remark;
        //        mbWallet.Credits = mbWallet.Credits + creditsType.Credits;
        //        if (mbWallet.Credits < 0)
        //        {
        //            response.SetContent(HttpStatus.error, "积分不足", "", ResponseType.josn);
        //            return response;
        //        }
        //        foreach (var item in mbGrade)
        //        {
        //            if (mbWallet.Credits > item.Credits)
        //            {
        //                mbBaseInfo.Grade = item.Grade;
        //                continue;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        var result = db.SqlServerClient.Ado.UseTran(() =>
        //          {
        //              crd.crd_CreditsGotSQLSugar.Insert(CreditsGot);
        //              mb.mb_WalletSugar.Update(mbWallet);
        //              mb.mb_BaseInfoSQLSugar.Update(mbBaseInfo);                   
        //          });
        //        if (result.IsSuccess)
        //        {

        //            response.SetContent(HttpStatus.ok, "积分添加成功", "", ResponseType.josn);
        //        }
        //        else
        //        {
        //            response.SetContent(HttpStatus.error, "积分添加失败", result.ErrorMessage, ResponseType.josn);

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        msg = "ERROR";
        //        AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
        //        response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
        //    }
        //    return response;
        ////}


    }
}