using ConfigCenter.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigCenter.Admin.Common
{
    [TypeFilter(typeof(LoginAttribute))]
    public class BaseController : Controller
    {
    }
}