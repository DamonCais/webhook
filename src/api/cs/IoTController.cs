using JVS_ADM.BasePage;
using System;
using System.Collections.Generic;
using ORM.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JVS_ADM.Common;
using JVS_ADM.BasePage;
using IoT.Net.Manager;
using ORM.SqlSugar.BLL;

namespace JVS_ADM.ControllersApi
{
    public class IoTController : MyApiController
    {
        dictManager dm = new dictManager();
        eqmManager eqm = new eqmManager();
        /// <summary>
        /// productKey=产品key&deviceName=设备名称&token=00002iloveyouruo
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage RegisterDevice(string productKey, string deviceName)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "S001_IoTController_RegisterDevice";
            var msg = "";
            try
            {
                if (string.IsNullOrEmpty(productKey) || string.IsNullOrEmpty(deviceName))
                {
                    response.SetContent(HttpStatus.error, "参数不齐", "ERROR", ResponseType.text);
                    return response;
                }

                ORM.SqlSugar.BLL.sysManager sysManager = new ORM.SqlSugar.BLL.sysManager();
                var model = sysManager.AliConfigSQLSugar.GetById(1);
                if (model == null)
                {
                    response.SetContent(HttpStatus.error, "没有阿里的配置信息", "ERROR", ResponseType.text);
                    return response;
                }
                var lotManager = new DeviceManager(model.AccessKey, model.SecretKey);
                var project = dm.dict_ProjectListSQLSugar.GetSingle(p => p.ProductKey == productKey);
                if (project == null)
                {
                    response.SetContent(HttpStatus.error, "没有找到ProductKey：" + productKey + "的项目", "ERROR_没有找到ProductKey", ResponseType.text);
                    return response;
                }
                var exit = eqm.eqm_EquipmentSugar.Count(p => p.DeviceName == deviceName);
                if (exit > 0)
                {
                    response.SetContent(HttpStatus.error, "已存在deviceName：" + deviceName + "的设备", "ERROR_已存在", ResponseType.text);
                    return response;
                }
                var equipment = new ORM.SqlSugar.Model.eqm.eqm_Equipment();
                equipment.EqmUID = project.ProjectCode + "_" + Guid.NewGuid().ToString();
                equipment.CorpCode="35003";
                equipment.DeviceName = deviceName;
                equipment.OnLine = false;
                equipment.ProjectCode = project.ProjectCode;
                equipment.ShopID = 1;
                equipment.RegisterDate = DateTime.Now;
                equipment.DeviceSecret = "";
                equipment.EquipmentName = project.ProjectName;
                var result = lotManager.RegisterDevice(productKey, deviceName, out msg);
                if (result)
                {
                    equipment.DeviceSecret = msg;
                    result = eqm.eqm_EquipmentSugar.Insert(equipment);
                    if (result)
                    {
                        response.SetContent(HttpStatus.ok, "设置成功", "OK_" + msg + "", ResponseType.text);
                        return response;
                    }
                    else
                    {
                        lotManager.DeleteDevice(productKey, deviceName, out msg);
                        response.SetContent(HttpStatus.error, "设置端服务失败", "ERROR_设置端服务失败", ResponseType.text);
                        return response;

                    }

                }
                else
                {
                    response.SetContent(HttpStatus.error, "设置失败", "ERROR_"+ msg + "", ResponseType.text);
                    return response;
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR_"+ex.Message+"";
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, msg, ResponseType.text);
            }

            return response;


        }
    }
}
