using ConfigCenter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ConfigCenter.Dto.ResultEnum;

namespace ConfigCenter.Admin.Common
{
    public class LoginFilterAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["name"] == null)
            {
                return false;
            }
            return true;
        }
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            //异步请求  
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                filterContext.Result = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new ResponseResult(ResultEnum.IsSuccess.失败, "请重新登录",errorType: ResultEnum.ErrorType.未登录),

                };
            }
            else
            {
                string MyAuthError = "/Login/Index";
                filterContext.Result = new RedirectResult(MyAuthError);
            }
            
        }
    }
}