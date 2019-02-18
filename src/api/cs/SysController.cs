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
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using JVS_PAY.WXPay;
using System.Xml;
using ORM.SqlSugar.BLL;

namespace JVS_ADM.ControllersApi
{
    public class SysController : MyApiController
    {

        ORM.SqlSugar.BLL.sysManager sysManager = new ORM.SqlSugar.BLL.sysManager();
        orgManager org = new orgManager();
        mbManager mb = new mbManager();
        crdManager crd = new crdManager();
        [HttpGet]
        public HttpResponseMessage GetVersionList(string ver)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "A020_SysController_GetVersionList";
            try
            {
                var curVersion = ver.ToDouble();
                var verModel = db.SqlServerClient.Queryable<ORM.SqlSugar.Model.sys.sys_AppVersion>().OrderBy(p => p.Version, OrderByType.Desc).Where(p => p.Version > curVersion).Take(1).Single();
                if (verModel != null)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(server.MapPath(verModel.PackageUrl));
                    var rbj = new { verModel = verModel, length = fileInfo.Length };
                    response.SetContent(HttpStatus.ok, "获取成功", rbj, ResponseType.josn);
                    return response;
                }
                response.SetContent(HttpStatus.error, "未获取到版本", null, ResponseType.josn);


            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage SendSms(string mobile)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C021_SysController_SendSms";
            try
            {
                ORM.SqlSugar.BLL.sysManager sysManager = new ORM.SqlSugar.BLL.sysManager();
                var model = sysManager.AliConfigSQLSugar.GetById(1);
                var instance = SMSManager.GetInstance(model.DyAccessKeyId, model.DyAccessKeySecret);
                var msgCode = JVS_ADM.Common.Utils.Number(5);
                var result = instance.SmsSendMesaage(mobile, "{\"code\":\"" + msgCode + "\"}", "SMS_147196393");
                if (result)
                {
                    if (dicRegCode.Keys.Contains(mobile))
                    {
                        dicRegCode[mobile] = msgCode;
                    }
                    else
                    {
                        dicRegCode.Add(mobile, msgCode);
                    }
                    response.SetContent(HttpStatus.ok, "发送成功", "", ResponseType.josn);

                }
                else
                {
                    response.SetContent(HttpStatus.error, "发送失败", "", ResponseType.josn);
                }

            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage code2Session(string appid, string js_code)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "A022_SysController_code2Session";
            try
            {
                var model = sysManager.WxConfigSQLSugar.GetById(appid);
                if (model == null)
                {
                    response.SetContent(HttpStatus.ok, "appid不在配置中", "", ResponseType.josn);
                    return response;
                }
                WebClient web = new WebClient();
                web.Encoding = Encoding.UTF8;
                var url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + model.AppSecret + "&js_code=" + js_code + "&grant_type=authorization_code";
                var res = web.DownloadString(url);
                response.SetContent(HttpStatus.ok, "获取成功", res, ResponseType.josn);
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AES_decrypt(string encryptedDataStr, string key, string iv)
        {



            var response = new MyHttpResponseMessage();
            response.apiNumber = "A023_SysController_AES_decrypt";
            try
            {
                encryptedDataStr = encryptedDataStr.Replace("2B%", "+");
                key = key.Replace("2B%", "+");
                iv = iv.Replace("2B%", "+");





                RijndaelManaged rijalg = new RijndaelManaged();
                //-----------------    
                //设置 cipher 格式 AES-128-CBC    




                rijalg.KeySize = 128;

                rijalg.Padding = PaddingMode.PKCS7;
                rijalg.Mode = CipherMode.CBC;

                rijalg.Key = Convert.FromBase64String(key);
                rijalg.IV = Convert.FromBase64String(iv);


                byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
                //解密    
                ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

                string result;

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            result = srDecrypt.ReadToEnd();
                            response.SetContent(HttpStatus.ok, "解密成功", result, ResponseType.josn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;

        }

        [HttpPost]
        public HttpResponseMessage SaveFile(string fileName)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C024_SysController_SaveFile";
            try
            {
                Stream st = context.Request.InputStream;
                var savePath = "/fileServer/File/" + fileName;
                if (!Directory.Exists(context.Request.MapPath("/fileServer/File//")))
                {
                    Directory.CreateDirectory(context.Request.MapPath("/fileServer/File//"));
                }
                string fileSavePath = context.Request.MapPath(savePath);
                byte[] buffer = new byte[st.Length];
                st.Read(buffer, 0, buffer.Length);
                if (File.Exists(fileSavePath))
                {
                    File.Delete(fileSavePath);
                }
                File.WriteAllBytes(fileSavePath, buffer);
                response.SetContent(HttpStatus.ok, "上传成功", savePath, ResponseType.josn);
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }
        [HttpPost]
        public HttpResponseMessage SaveImage(string ImageName, int width, int height)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C025_SysController_SaveImage";
            try
            {
                var base64 = context.Request["base64"].ToString();

                var folder = "upload";
                var base64_Thumbnail = base64;
                var ibyte = base64_Thumbnail.Thumbnail(width, height);

                var timeSpan = DateTime.Now.ToString("yyyyMMddhhmmss");


                var savePath = "/fileServer/image/" + folder + "/" + timeSpan + "/thumbnail/";
                string path_thumbnail = context.Server.MapPath(savePath);

                if (!Directory.Exists(path_thumbnail))
                {
                    Directory.CreateDirectory(path_thumbnail);
                }

                var imgStream1 = new MemoryStream(ibyte);

                Image img1 = Image.FromStream(imgStream1);


                var spath_thumbnai = path_thumbnail + ImageName;

                if (File.Exists(spath_thumbnai))
                {
                    File.Delete(spath_thumbnai);
                }
                img1.Save(spath_thumbnai);
                img1.Dispose();
                response.SetContent(HttpStatus.ok, "上传成功", savePath + ImageName, ResponseType.josn);
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage WxPaySign(string MemberCode, string AppID, string openid, string CardNo, string type)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C027_SysController_PaySign";
            try
            {
                var model = sysManager.WxConfigSQLSugar.GetById(AppID);
                if (model == null)
                {
                    response.SetContent(HttpStatus.ok, "appid不在配置中", "", ResponseType.josn);
                    return response;
                }

                var order_no = CardNo; //订单号
                var order_amount = 0M; //订单金额
                var user_name = ""; //支付用户名
                var subject = ""; //备注说明
                var UserOpenId = openid; //备注说明
                var trans_type = string.Empty; //交易类型1实物2虚拟
                if (type == "card")
                {
                    subject = "购买套餐卡";
                    var Card = mb.mb_CardList.GetList(p => p.CardNo == order_no).First();

                    if (Card == null)
                    {
                        response.SetContent(HttpStatus.ok, "订单记录不存在", "", ResponseType.josn);
                        return response;
                    }
                    order_amount = Card.Amount.ToDecimal();
                    user_name = Card.MemberCode;

                }
                if (type == "Balance")
                {
                    subject = "充值余额";
                    var Balance = crd.crd_BalanceLogSQLSugar.GetList(p => p.BalanceNo == order_no).First();

                    if (Balance == null)
                    {
                        response.SetContent(HttpStatus.ok, "订单记录不存在", "", ResponseType.josn);
                        return response;
                    }

                    order_amount = Balance.OriginalBalance.ToDecimal();
                    user_name = Balance.MemberCode;
                }
                //===============================请求参数==================================
                //时间戳 
                var TimeStamp = TenpayUtil.getTimestamp();
                //随机字符串 
                var NonceStr = TenpayUtil.getNoncestr();

                //创建支付应答对象
                var packageReqHandler = new RequestHandler(System.Web.HttpContext.Current);
                //初始化
                packageReqHandler.init();

                //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
                packageReqHandler.setParameter("body", subject); //商品信息 127字符
                packageReqHandler.setParameter("appid", model.AppID);
                packageReqHandler.setParameter("mch_id", model.MchID);
                packageReqHandler.setParameter("nonce_str", NonceStr.ToLower());
                packageReqHandler.setParameter("notify_url", model.Notify_url);
                packageReqHandler.setParameter("openid", UserOpenId);
                packageReqHandler.setParameter("out_trade_no", order_no); //商家订单号
                packageReqHandler.setParameter("spbill_create_ip", System.Web.HttpContext.Current.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                packageReqHandler.setParameter("total_fee", (order_amount * 100).ToInt32().ToString()); //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.setParameter("trade_type", "JSAPI");
                if (!string.IsNullOrEmpty(subject))
                    packageReqHandler.setParameter("attach", subject);//自定义参数 127字符



                #region sign===============================
                var Sign = packageReqHandler.CreateMd5Sign("key", model.MchSecret);
                //  LogUtil.WriteLog("WeiPay 页面  sign：" + Sign);
                #endregion

                #region 获取package包======================
                packageReqHandler.setParameter("sign", Sign);

                string data = packageReqHandler.parseXML();
                //   LogUtil.WriteLog("WeiPay 页面  package（XML）：" + data);
                LogManager.Logs["微信支付"].WriteLine("data:" + data);
                string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
                //  LogUtil.WriteLog("WeiPay 页面  package（Back_XML）：" + prepayXml);
                LogManager.Logs["微信支付"].WriteLine("prepayXml:" + prepayXml);
                //获取预支付ID
                var xdoc = new XmlDocument();
                xdoc.LoadXml(prepayXml);
                XmlNode xn = xdoc.SelectSingleNode("xml");
                XmlNodeList xnl = xn.ChildNodes;
                var Package = "";
                if (xnl.Count > 7)
                {
                    var PrepayId = xnl[7].InnerText;
                    Package = string.Format("prepay_id={0}", PrepayId);

                }
                if (string.IsNullOrEmpty(Package))
                {
                    response.SetContent(HttpStatus.error, "支付包生成失败", "", ResponseType.josn);
                    return response;
                }
                LogManager.Logs["微信支付"].WriteLine("Package:" + Package);
                #endregion

                #region 设置支付参数 输出页面  该部分参数请勿随意修改 ==============
                var paySignReqHandler = new RequestHandler(System.Web.HttpContext.Current);
                paySignReqHandler.setParameter("appId", model.AppID);
                paySignReqHandler.setParameter("timeStamp", TimeStamp);
                paySignReqHandler.setParameter("nonceStr", NonceStr);
                paySignReqHandler.setParameter("package", Package);
                paySignReqHandler.setParameter("signType", "MD5");
                var PaySign = paySignReqHandler.CreateMd5Sign("key", model.MchSecret);
                LogManager.Logs["微信支付"].WriteLine("PaySign:" + PaySign);
                var rbj = new { appId = model.AppID, package = Package, timeStamp = TimeStamp, nonceStr = NonceStr, PaySign = PaySign };
                response.SetContent(HttpStatus.ok, "获取支付参数成功", rbj, ResponseType.josn);
                #endregion
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.sys, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, null, ResponseType.josn);
            }
            return response;
        }


        [HttpGet]
        public HttpResponseMessage PayBalance(string MemberCode, string CardNo)
        {
            var response = new MyHttpResponseMessage();
            response.apiNumber = "C028_SysControllerr_PayBalance";
            try
            {

                if (CardNo.Contains("Card"))
                {
                    var Card = mb.mb_CardList.GetById(CardNo);
                    var vCard = org.org_VirtualCardSugar.GetById(Card.VCardNo);


                    var wallet = mb.mb_WalletSugar.GetById(MemberCode);
                    if (wallet == null)
                    {
                        response.SetContent(HttpStatus.error, "没找到您的余额账户！", Card, ResponseType.josn);
                        return response;
                    }
                    if (wallet.Balance < Card.Amount)
                    {
                        response.SetContent(HttpStatus.error, "您的余额不足，请充值！", Card, ResponseType.josn);
                        return response;
                    }
                    var result = db.SqlServerClient.Ado.UseTran(() =>
                    {
                        var f = mb.FinshRecharg(Card, vCard);
                        if (f)
                        {
                            Card.Status = 3;
                            Card.PayNo = "Balance" + JVS_ADM.Common.Utils.GetRamCode();
                            Card.PayTime = DateTime.Now;
                            mb.mb_CardList.Update(Card);
                            var crdLog = new ORM.SqlSugar.Model.crd.crd_BalanceLog();
                            crdLog.BalanceNo = "Balance" + Utils.GetRamCode() + "";
                            crdLog.Balance = -Card.Amount;
                            crdLog.OriginalBalance = -Card.Amount;
                            crdLog.DonationBalance = 0;
                            crdLog.BalanceID = 0;
                            crdLog.MemberCode = MemberCode;
                            crdLog.BalanceType = 2;
                            crdLog.Remark = "";
                            crdLog.TradeNo = "";
                            crdLog.PayTime = null;
                            crdLog.PayWay = 0;
                            crdLog.Status = 3;
                            crdLog.CreateDate = DateTime.Now;
                            crd.crd_BalanceLogSQLSugar.Insert(crdLog);
                            wallet.Balance = wallet.Balance - Card.Amount;
                            mb.mb_WalletSugar.Update(wallet);
                            if (mb.mb_MemberCorpSugar.Count(p => p.MemberCode == Card.MemberCode && p.CorpCode == vCard.CorpCode) <= 0)
                            {
                                var mcorp = new ORM.SqlSugar.Model.mb.mb_MemberCorp();
                                mcorp.CorpCode = vCard.CorpCode;
                                mcorp.MemberCode = Card.MemberCode;
                                mcorp.ShopID = 0;
                                mcorp.CreateTime = DateTime.Now;
                                mb.mb_MemberCorpSugar
                                    .Insert(mcorp);
                            }
                        }
                    });

                    if (result.IsSuccess)
                    {
                        response.SetContent(HttpStatus.ok, "支付完成", Card, ResponseType.josn);
                        return response;

                    }
                    else
                    {
                        response.SetContent(HttpStatus.error, "余额扣款失败", Card, ResponseType.josn);
                        return response;

                    }
                }



                response.SetContent(HttpStatus.error, "余额支付失败", "", ResponseType.josn);
                return response;
            }
            catch (Exception ex)
            {
                AddErrorLog(LogEnum.member, ex.Message, ex.StackTrace);
                response.SetContent(HttpStatus.error, ex.Message, ex.Message, ResponseType.josn);
                return response;
            }
        }
    }
}
