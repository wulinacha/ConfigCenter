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
    [Route("api/appsetting")]
    [ApiController]
    public class AppsettingController : ControllerBase
    {
        [HttpGet("appsettings/{AppId}")]
        public dynamic Get([FromRoute]int appId)
        {
            return new GetAppSettingsResponse() { AppSettings = AppSettingBusiness.GetAppSettings(appId) };
        }
    }
}
