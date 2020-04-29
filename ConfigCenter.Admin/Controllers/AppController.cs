using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using System.Text;
using ConfigCenter.Admin.Common;
using ConfigCenter.Admin;
using static ConfigCenter.Dto.UserEnum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Webdiyer.AspNetCore;

namespace ConfigCenter.Admin.Controllers    
{
    [Route("App")]
    public class AppController : BaseController 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public AppController(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("Index")]
        public ActionResult Index(int pageindex = 1, string kword = "")
        {
            long totalItem;
            int pageSize = 12;
            string sarchKword;

             sarchKword = GetProjectNameIfAdminOrSpeac(kword); 

             ViewData["rolename"] = _session.GetString("rolename");

            var pageList = AppBusiness.GetApps(pageindex, pageSize, sarchKword, out totalItem).ToPagedList(pageindex, pageSize,totalItem);

            return View(pageList);
        }

        private string GetProjectNameIfAdminOrSpeac(string kword)
        {
            if (_session.GetString("rolename") != RoleEnum.管理员.ToString())//非管理员不提供搜索
                kword = _session.GetString("name");
            return kword;
        }

        /// <summary>
        /// 密码重新加密
        /// </summary>
        /// <returns></returns>
        //public JsonResult InitAccountPassword()
        //{
        //    ResponseResult responseResult = null;
        //    try
        //    {
        //        List<AccountDto> list = AccountBusiness.GetAccountList();
        //        //盐值
        //        string salt = LoginHelper.GetGuidShort();
        //        list.ForEach(e =>
        //        {
        //            var model = e;
        //            model.Password = LoginHelper.GetSaltPassword(e.Password, salt);
        //            model.Salt = salt;
        //            AccountBusiness.SaveUser(model);
        //        });
        //        responseResult = new ResponseResult(ResultEnum.IsSuccess.成功, "成功初始化");
        //    }
        //    catch (Exception ex)
        //    {

        //        responseResult= new ResponseResult(ResultEnum.IsSuccess.失败, "初始化失败");
        //    }
           
        //    return Json(responseResult);
        //}
        [HttpGet("GetAppByid")]
        public JsonResult GetAppById(int id)
        {
            var appdto = AppBusiness.GetAppById(id);
            appdto.Envs = new List<ConfigEnvironment>();
            appdto.Envs.Add(new ConfigEnvironment() { Name="DEV",Desc= "DEV(开发环境)" });
            appdto.Envs.Add(new ConfigEnvironment() { Name = "ALPHA", Desc = "ALPHA(测试环境)" });
            appdto.Envs.Add(new ConfigEnvironment() { Name = "PROD", Desc = "PROD(正式环境)" });
            return Json(new ResponseResult(ResultEnum.IsSuccess.成功, "获取成功", appdto));
            //return Json(AppBusiness.GetAppById(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost("DeleteAppById")]
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
            return Json(responseResult); 
        }
        [HttpPost("SaveApp")]
        public JsonResult SaveApp(AppDto appDto)
        {
            ResponseResult responseResult;
            try
            {
                AppBusiness.SaveApp(appDto);

                AddAccountIfNo(appDto);
                responseResult = new ResponseResult(true, "");
            }
            catch (Exception ex)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult);
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
