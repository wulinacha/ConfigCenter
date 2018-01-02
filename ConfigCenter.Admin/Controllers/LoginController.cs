using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Webdiyer.WebControls.Mvc;
using System.Text;
using ConfigCenter.Admin.Common;

namespace ConfigCenter.Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["name"] != null && !Request.IsAjaxRequest())
            {
                return GoToDefualt();
            }
            return View();
        }
        
        // GET: App
        [HttpPost]
        public ActionResult Index(string name, string password)
        {
            if (Session["name"] != null && !Request.IsAjaxRequest())
            {
                return GoToDefualt();
            }
            ResponseResult responseResult = new ResponseResult(true,null);
            if (name == null || password == null)
            {
                responseResult = new ResponseResult(false, "用户名和密码不能为空");
                return Json(responseResult, JsonRequestBehavior.AllowGet);
            }
            AccountDto userDto=AccountBusiness.GetAccountByName(name);
            if (userDto == null)
            {
                responseResult = new ResponseResult(false, "用户名不正确");
                return Json(responseResult, JsonRequestBehavior.AllowGet);
            }
            string saltPassword = LoginHelper.GetSaltPassword(password, userDto.Salt);
            if (userDto.Name != name || userDto.Password != saltPassword)
            {
                responseResult = new ResponseResult(false, "密码不正确");
                return Json(responseResult, JsonRequestBehavior.AllowGet);
            }

            LoginHelper.Login(name,password, userDto.RoleId.ToString());
            
            responseResult = new ResponseResult(true, "/App/Index");
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult logout()
        {
            LoginHelper.Logout();
            return Redirect("/Login/Index");
        }

        private ActionResult GoToDefualt()
        {
            return Redirect("/App/Index");
        }
    }
}
