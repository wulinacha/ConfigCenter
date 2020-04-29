using ConfigCenter.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ConfigCenter.Admin.Common
{
    public class LoginAttribute : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public LoginAttribute(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (_session.GetString("name") != null)
            {
                //filterContext.Result = new JsonResult(new ResponseResult(ResultEnum.IsSuccess.成功,"成功"));
                return;
            }
            else 
            {
                //异步请求  
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    filterContext.Result = new JsonResult(new ResponseResult(ResultEnum.IsSuccess.失败, "请重新登录", errorType: ResultEnum.ErrorType.未登录));
                }
                else
                {
                    string MyAuthError = "/Login";
                    filterContext.Result = new RedirectResult(MyAuthError);
                }
            }

        }
    }
}
