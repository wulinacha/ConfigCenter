using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigCenter.Admin.Common;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigCenter.Admin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DefaultController : Controller
    {
        [HttpGet("logout")]
        public ActionResult logout()
        {
            LoginHelper.Logout();
            return Redirect("/Login");
        }
        [HttpGet("ChangePassword")]
        public ActionResult ChangePassword() {
            List<AccountDto> accounts= AccountBusiness.GetAccountList();
            accounts.ForEach(account =>
            {
                string saltPassword = LoginHelper.GetSaltPassword(account.Name, account.Salt);
                account.Password = saltPassword;
                AccountBusiness.SaveUser(account);
            });
            return Json("成功");
        }
    }
}