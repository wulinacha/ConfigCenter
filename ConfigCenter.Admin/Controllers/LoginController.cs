using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using System.Text;
using ConfigCenter.Admin.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ConfigCenter.Admin.Models;
using ConfigCenter.Admin.Models.Request;

namespace ConfigCenter.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public LoginController(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (_session.GetString("name") != null && !Request.IsAjaxRequest())
            {
                return GoToDefualt();
            }
            return View();
        }
        
        // GET: App
        [HttpPost]
        public ActionResult Index([FromForm]LoginRequest login)
        {
            if (_session.GetString("name") != null && !Request.IsAjaxRequest())
            {
                return GoToDefualt();
            }
            ResponseResult responseResult = new ResponseResult(true,null);
            if (login.name == null || login.password == null)
            {
                responseResult = new ResponseResult(false, "用户名和密码不能为空");
                return Json(responseResult);
            }
            AccountDto userDto=AccountBusiness.GetAccountByName(login.name);
            if (userDto == null)
            {
                responseResult = new ResponseResult(false, "用户名不正确");
                return Json(responseResult);
            }
            string saltPassword = LoginHelper.GetSaltPassword(login.password, userDto.Salt);
            if (userDto.Name != login.name || userDto.Password != saltPassword)
            {
                responseResult = new ResponseResult(false, "密码不正确");
                return Json(responseResult);
            }

            LoginHelper.Login(new LoginModel { 
                name= login.name,
                password= login.password,
                rolename= userDto.RoleId.ToString()
            },_session);
            
            responseResult = new ResponseResult(true, "/App/Index");
            return Json(responseResult);
        }



        private ActionResult GoToDefualt()
        {
            return Redirect("/App/Index");
        }
    }
}
