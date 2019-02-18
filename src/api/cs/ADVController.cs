using JVS_ADM.BasePage;
using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ORM.SqlSugar.BLL;

namespace JVS_ADM.ControllersApi
{
    public class ADVController : MyApiController
    {
        advManager adv = new advManager();

        [HttpGet]
        public HttpResponseMessage AdvHeadCreate(string AdCode, string CorpCode,string ADTitle,int AdType,string SubmitBy)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C090_ADVController_AdvHeadCreate";
            var msg = "";
            try
            {
                var advHead = new ORM.SqlSugar.Model.adv.adv_Head();
                advHead.AdCode = AdCode;
                advHead.CorpCode = CorpCode;
                advHead.ADTitle = ADTitle;
                advHead.AdType = AdType;
                advHead.SubmitBy = SubmitBy;
                advHead.SubmitTime = DateTime.Now;

               var result= adv.adv_HeadSQLSugar.Insert(advHead);

                if (result)
                {
                    response.SetContent(HttpStatus.ok, "增加成功", advHead, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "增加失败", result, ResponseType.josn);
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
        public HttpResponseMessage AdvListCreate(string AdCode, int SortID, bool OnLine, string AdLink, string AdContents, string AdImageLink, string AdImageURL, string AdImageTitle)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C091_ADVController_AdvListCreate";
            var msg = "";
            try
            {
                var advList = new ORM.SqlSugar.Model.adv.adv_List();
                var advHead = adv.adv_HeadSQLSugar.GetSingle(p => p.AdCode == AdCode);
                advList.AdCode = AdCode;
                advList.AdLink = AdLink;
                advList.OnLine = OnLine;
                advList.SortID = SortID;
                if ( advHead.AdType == 0)
                {
                    advList.AdContents = AdContents;
                    advList.AdImageLink = null;
                    advList.AdImageTitle = null;
                    advList.AdImageURL = null;
                }

                else if (advHead.AdType == 1)
                {
                    advList.AdContents = null;
                    advList.AdImageLink = AdImageLink;
                    advList.AdImageTitle = AdImageTitle;
                    advList.AdImageURL = AdImageURL;

                }

                var result = adv.adv_ListSQLSugar.Insert(advList);

                if (result)
                {
                    response.SetContent(HttpStatus.ok, "增加成功", advList, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "增加失败", result, ResponseType.josn);
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
        public HttpResponseMessage AdvHeadRevise(string AdCode, string ADTitle, int AdType, string SubmitBy,bool isDefault)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C092_ADVController_AdvHeadRevise";
            var msg = "";
            try
            {
                var advHead = adv.adv_HeadSQLSugar.GetSingle(p=>p.AdCode==AdCode);
              
                advHead.ADTitle = ADTitle;
                advHead.AdType = AdType;
                advHead.IsDefault = isDefault;
                advHead.SubmitBy = SubmitBy;
                advHead.SubmitTime = DateTime.Now;             

                var result = adv.adv_HeadSQLSugar.Update(advHead);
  
                if (result)
                {
                    response.SetContent(HttpStatus.ok, "修改成功", advHead, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "修改失败", result, ResponseType.josn);
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
        public HttpResponseMessage AdvListRevise(string AdCode,int SortID, bool OnLine, string AdLink, string AdContents, string AdImageLink, string AdImageURL, string AdImageTitle)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C093_ADVController_AdvListRevise";
            var msg = "";
            try
            {
                var advHead = adv.adv_HeadSQLSugar.GetSingle(p => p.AdCode == AdCode);

                var advList = adv.adv_ListSQLSugar.GetSingle(p => p.AdCode == AdCode && p.SortID == SortID);

                advList.AdLink = AdLink;
                advList.OnLine = OnLine;

                if (advHead.AdType == 0)
                {
                    advList.AdContents = AdContents;
                    advList.AdImageLink = null;
                    advList.AdImageTitle = null;
                    advList.AdImageURL = null;
                }

                else if (advHead.AdType == 1)
                {
                    advList.AdContents = null;
                    advList.AdImageLink = AdImageLink;
                    advList.AdImageTitle = AdImageTitle;
                    advList.AdImageURL = AdImageURL;
                }


                var result = adv.adv_ListSQLSugar.Update(advList);


                if (result)
                {
                    response.SetContent(HttpStatus.ok, "修改成功", advList, ResponseType.josn);
                }
                else
                {
                    response.SetContent(HttpStatus.error, "修改失败", result, ResponseType.josn);
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
        public HttpResponseMessage GetAdvInfo(string AdCode)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C094_ADVController_GetAdvInfo";
            var msg = "";
            try
            {

                var advInfoList = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.adv.adv_Head, ORM.SqlSugar.Model.adv.adv_List>((h, l) => new object[] { JoinType.Right, h.AdCode == l.AdCode }).Select((h, l) => new { AdCode = h.AdCode, CorpCode = h.CorpCode, ADTitle = h.ADTitle, AdType = h.AdType, IsDefault = h.IsDefault, SubmitBy = h.SubmitBy, SubmitTime = h.SubmitTime, SortID = l.SortID, OnLine = l.OnLine, AdLink = l.AdLink, AdContents = l.AdContents, AdImageLink = l.AdImageLink, AdImageURL = l.AdImageURL, AdImageTitle = l.AdImageTitle }).ToList();

                advInfoList = advInfoList.Where(p => p.AdCode == AdCode).ToList();

                response.SetContent(HttpStatus.ok, "获取成功", advInfoList, ResponseType.josn);

            }

            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.adv, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

    }
}
