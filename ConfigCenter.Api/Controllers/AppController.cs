using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigCenter.Api.Controllers
{
    [Route("api/app")]
    [ApiController]
    public class AppController : ControllerBase
    {
        [HttpGet("version/{AppId}")]
        public dynamic Get([FromRoute]string appId)
        {
            return new GetAppVersionResponse() { AppDto = AppBusiness.GetAppVersion(appId) };
        }
    }
}