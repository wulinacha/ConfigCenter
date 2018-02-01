using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Webdiyer.WebControls.Mvc;
using ConfigCenter.Admin.Common;
using ConfigCenter.Common.Extensions;
using System.IO;
using System.Data;
using ConfigCenter.Common;
using NPOI.XSSF.UserModel;
using System.Text;

namespace ConfigCenter.Admin.Controllers
{
    public class AppSettingController : BaseController
    {
        // GET: App
        public ActionResult Index(int pageindex = 1, int appId = 0, string kword = "")
        {
            long totalItem;
            var dto = AppSettingBusiness.GetAppSettings(appId, pageindex,20, kword, out totalItem);
            var app = AppBusiness.GetAppById(appId);
            ViewBag.appid = appId;
            if (app != null)
            {
                ViewData["Evn"] = app.Environment;
                ViewData["ProjectName"] = app.AppId;
            }
            else
            {
                ViewData["Evn"] = "无法识别";
                ViewData["ProjectName"] = "无法识别";
            }
            return View(new PagedList<AppSettingDto>(dto, pageindex, 20, (int)totalItem));
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upload()
        {
            string fileName = "配置.xlsx";
            try
            {
                var filePath = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(fileName));
                //创建文件流  
                FileStream myfs = new FileStream(filePath, FileMode.Open);

                return File(myfs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch
            {
                return Content("下载异常 ！", "text/plain");
            }
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Import(int appid)
        {
            try
            {
                int appIdSelf= appid;
                //存入本地
                var file = Request.Files[0];

                string sheet = "配置";
                List<AppSettingDto> appSettings = new List<AppSettingDto>();
                //传入参数,将excel转换为Datatable，格式文件名加上固定模版Sheet1
                DataTable dt = Entities.ExcelToDataTableHelper(file, sheet);
                //循环这个集合并添加到数据表
                foreach (DataRow item in dt.Rows)
                {
                    AppSettingDto appSetting = new AppSettingDto();
                    appSetting.ConfigKey = item[0].ToString();
                    appSetting.ConfigValue = item[1].ToString();
                    appSettings.Add(appSetting);
                }

               AppSettingBusiness.SaveAppSettings(appSettings, appIdSelf);
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult(false,"上传格式有问题！"));
            }

            return Json(new ResponseResult(true, "上传成功！"));
        }

        [HttpPost]
        public ActionResult DeleteAlllAppSetting(int appid)
        {
            try
            {
                AppSettingBusiness.DeleteSettingByAppid(appid);
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult(false, "删除所有失败"),
               JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseResult(true, "删除所有成功"),
               JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult Export(int appId)
        {
            //获取list数据

            var checkList = AppSettingBusiness.GetAppSettings(appId); //db.InfoTables.Where(r => r.ProjectName != null).Select(r => new { r.ProjectName, r.InfoTypes, r.field, r.fieldtxt }).ToList();
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("配置键");
            row1.CreateCell(1).SetCellValue("配置值");
            //....N行

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < checkList.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(checkList[i].ConfigKey.ToString());
                rowtemp.CreateCell(1).SetCellValue(checkList[i].ConfigValue.ToString());
                //....N行
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            DateTime dt = DateTime.Now;
            string dateTime = dt.ToString("yyMMddHHmmssfff");
            string fileName = "配置" + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);
        }

        public JsonResult GetAppSettingById(int id)
        {
            return Json(new ResponseResult(true,"获取成功", AppSettingBusiness.GetAppSettingById(id)),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAppSettingById(int id)
        {
            ResponseResult responseResult;
            try
            {
                var result = AppSettingBusiness.DeleteAppSettingById(id);
                responseResult = new ResponseResult(result, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAppSetting(AppSettingDto appSettingDto)
        {
            ResponseResult responseResult;
            bool isAdd = appSettingDto.Id==0;
            try
            {
                var isConfiKeyExisted = ConfiKeyExisted(appSettingDto);
                if (isAdd)
                {
                    if (isConfiKeyExisted)
                    {
                        AppSettingBusiness.SaveAppSetting(appSettingDto);
                        responseResult = new ResponseResult(ResultEnum.IsSuccess.成功, "新增配置成功");
                    }
                    else
                    {
                        responseResult = new ResponseResult(ResultEnum.IsSuccess.失败, "已存在该项配置，请考虑清楚再添加！");
                    }
                }
                else
                {
                    AppSettingBusiness.SaveAppSetting(appSettingDto);
                    responseResult = new ResponseResult(ResultEnum.IsSuccess.成功, "新增配置成功");
                }
            }
            catch (Exception ex)
            {
                responseResult = new ResponseResult(ResultEnum.IsSuccess.失败, "添加配置异常，请联系管理员！"+ appSettingDto.AppId);
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        private static bool ConfiKeyExisted(AppSettingDto appSettingDto)
        {
            var appSetting = AppSettingBusiness.GetAppSettingByKeyAndAppId(appSettingDto.ConfigKey, appSettingDto.AppId);
            return appSetting.IsNull();
        }
    }
}
