using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Webdiyer.WebControls.Mvc;
using static ConfigCenter.Dto.UserEnum;
using System.Text;
using ConfigCenter.Admin.Common;
using ConfigCenter.Admin;
using static ConfigCenter.Dto.ResultEnum;

namespace ConfigCenter.Admin.Controllers
{
    public class AppController : BaseController 
    {
        // GET: App
        public ActionResult Index(int pageindex = 1, string kword = "")
        {
            long totalItem;
            string sarchKword;

            sarchKword = GetProjectNameIfAdminOrSpeac(kword);

            ViewData["roleName"] = Session["roleName"];

            var dto = AppBusiness.GetApps(pageindex, 20, sarchKword, out totalItem);

            return View(new PagedList<AppDto>(dto, pageindex, 20, (int)totalItem));
        }

        private string GetProjectNameIfAdminOrSpeac(string kword)
        {
            if (Session["roleName"].ToString() != RoleEnum.管理员.ToString())//非管理员不提供搜索
                kword = Session["name"].ToString();
            return kword;
        }

        /// <summary>
        /// 密码重新加密
        /// </summary>
        /// <returns></returns>
        public JsonResult InitAccountPassword()
        {
            ResponseResult responseResult = null;
            try
            {
                List<AccountDto> list = AccountBusiness.GetAccountList();
                //盐值
                string salt = LoginHelper.GetGuidShort();
                list.ForEach(e =>
                {
                    var model = e;
                    model.Password = LoginHelper.GetSaltPassword(e.Password, salt);
                    model.Salt = salt;
                    AccountBusiness.SaveUser(model);
                });
                responseResult = new ResponseResult(ResultEnum.IsSuccess.成功, "成功初始化");
            }
            catch (Exception ex)
            {

                responseResult= new ResponseResult(ResultEnum.IsSuccess.失败, "初始化失败");
            }
           
            return Json(responseResult);
        }

        public JsonResult GetAppById(int id)
        {
            return Json(new ResponseResult(ResultEnum.IsSuccess.成功, "获取成功", AppBusiness.GetAppById(id)), JsonRequestBehavior.AllowGet);
            //return Json(AppBusiness.GetAppById(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAppById(int id)
        {
            ResponseResult responseResult;
            try
            {
                var result = AppBusiness.DeleteAppById(id);
                responseResult = new ResponseResult(result, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveApp(AppDto appDto)
        {
            ResponseResult responseResult;
            try
            {
                AppBusiness.SaveApp(appDto);

                AddAccountIfNo(appDto);
                responseResult = new ResponseResult(true, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 如果该项目没有账号，则添加
        /// </summary>
        /// <param name="appDto"></param>
        private static void AddAccountIfNo(AppDto appDto)
        {
            if (AccountBusiness.GetAccountByName(appDto.AppId) == null)//如果不存在则添加用户
            {
                #region 密码加密加盐
                string salt = LoginHelper.GetGuidShort();
                string password = LoginHelper.GetSaltPassword(appDto.AppId, salt);
                #endregion
                AccountBusiness.SaveUser(new AccountDto
                {
                    Name = appDto.AppId,
                    Password = password,
                    RoleId = RoleEnum.普通人员,
                    Salt = salt
                });
            }
        }

        
    }
}
