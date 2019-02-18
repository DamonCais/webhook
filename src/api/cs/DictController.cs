using JVS_ADM.BasePage;
using ORM.SqlSugar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JVS_ADM.ControllersApi
{
    public class DictController : MyApiController
    {
        dictManager dm = new dictManager();
        [HttpGet]
        public HttpResponseMessage QueryProject(int Online)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C70_DictController_QueryProject";
            var msg = "";
            try
            {
                var list = dm.dict_ProjectListSQLSugar.GetList(p => p.Online == Online);
                response.SetContent(HttpStatus.ok, "获取成功", list, ResponseType.josn);
            }
            catch (Exception ex)
            {
                msg = "ERROR";
                AddErrorLog(LogEnum.dict, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.josn);
            }
            return response;
        }

      
    }
}
