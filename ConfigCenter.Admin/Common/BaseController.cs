using ConfigCenter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConfigCenter.Admin.Common
{
    //[LoginFilter]
    public class BaseController : Controller
    {
        public string GetAccountName() {
            return Session["Name"].ToString();
        }
    }
}