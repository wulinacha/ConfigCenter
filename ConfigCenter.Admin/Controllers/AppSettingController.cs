using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Webdiyer.WebControls.Mvc;
using ConfigCenter.Admin.Common;
using ConfigCenter.Common.Extensions;
using static ConfigCenter.Dto.ResultEnum;

namespace ConfigCenter.Admin.Controllers
{
    public class AppSettingController : BaseController
    {
        // GET: App
        public ActionResult Index(int pageindex = 1, int appId = 0, string kword = "")
        {
            long totalItem;
            var dto = AppSettingBusiness.GetAppSettings(appId, pageindex,20, kword, out totalItem);
            var app = AppBusiness.GetAppById(appId);
            if (app != null)
            {
                ViewData["Evn"] = AppBusiness.GetAppById(appId).Environment;
                ViewData["ProjectName"] = AppBusiness.GetAppById(appId).AppId;
            }
            else
            {
                ViewData["Evn"] = "无法识别";
                ViewData["ProjectName"] = "无法识别";
            }
            return View(new PagedList<AppSettingDto>(dto, pageindex, 20, (int)totalItem));
        }

        public JsonResult GetAppSettingById(int id)
        {
            return Json(new ResponseResult(true,"获取成功", AppSettingBusiness.GetAppSettingById(id)),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAppSettingById(int id)
        {
            ResponseResult responseResult;
            try
            {
                var result = AppSettingBusiness.DeleteAppSettingById(id);
                responseResult = new ResponseResult(result, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAppSetting(AppSettingDto appSettingDto)
        {
            ResponseResult responseResult;
            bool isAdd = appSettingDto.Id==0;
            try
            {
                var isConfiKeyExisted = ConfiKeyExisted(appSettingDto);
                if (isAdd)
                {
                    if (isConfiKeyExisted)
                    {
                        AppSettingBusiness.SaveAppSetting(appSettingDto);
                        responseResult = new ResponseResult(IsSuccess.成功, "新增配置成功");
                    }
                    else
                    {
                        responseResult = new ResponseResult(IsSuccess.失败, "已存在该项配置，请考虑清楚再添加！");
                    }
                }
                else
                {
                    AppSettingBusiness.SaveAppSetting(appSettingDto);
                    responseResult = new ResponseResult(IsSuccess.成功, "新增配置成功");
                }
            }
            catch (Exception ex)
            {
                responseResult = new ResponseResult(IsSuccess.失败, "添加配置异常，请联系管理员！"+ appSettingDto.AppId);
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        private static bool ConfiKeyExisted(AppSettingDto appSettingDto)
        {
            var appSetting = AppSettingBusiness.GetAppSettingByKeyAndAppId(appSettingDto.ConfigKey, appSettingDto.AppId);
            return appSetting.IsNull();
        }
    }
}
