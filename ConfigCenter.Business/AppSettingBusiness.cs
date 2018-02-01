﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ConfigCenter.Dto;
using ConfigCenterConnection;
using System.Data.Common;

namespace ConfigCenter.Business
{
    public class AppSettingBusiness
    {

        public static List<AppSettingDto> GetAppSettings(int appId)
        {
            return Mapper.Map<List<AppSetting>, List<AppSettingDto>>(AppSetting.Query("WHERE AppId=@0", appId).ToList());
        }

        public static List<AppSettingDto> GetAppSettings(int appId, int pageIndex, int pageSize, string kword, out long totalPage)
        {
            var page = AppSetting.Page(pageIndex, pageSize, "WHERE AppId=@0 AND ConfigKey LIKE @1",
                new object[] { appId, "%" + kword + "%" });
            totalPage = page.TotalItems;
            return Mapper.Map<List<AppSetting>, List<AppSettingDto>>(page.Items);
        }

        public static AppSettingDto GetAppSettingByKeyAndAppId(String configKey,int AppId)
        {
            return Mapper.Map<AppSetting, AppSettingDto>(AppSetting.SingleOrDefault("WHERE ConfigKey=@0 and AppId=@1",
                configKey, AppId));
        }

        public static AppSettingDto GetAppSettingById(int id)
        {
            return Mapper.Map<AppSetting, AppSettingDto>(AppSetting.SingleOrDefault("WHERE Id=@0", id));
        }

        public static void SaveAppSetting(AppSettingDto appSettingDto)
        {
            var appSetting = Mapper.Map<AppSettingDto, AppSetting>(appSettingDto);
            appSetting.Save();

            var app = App.SingleOrDefault(appSettingDto.AppId);
            if (app != null)
            {
                app.Version = DateTime.Now.ToString("yyyyMMddHHmmss");
                app.Save();
            }
        }

        public static void SaveAppSettings(List<AppSettingDto> appSettingDtos,int appId)
        {
            foreach (var appSettingDto in appSettingDtos)
            {
                var appSetting = Mapper.Map<AppSettingDto, AppSetting>(appSettingDto);
                appSetting.AppId = appId;
                appSetting.Save();
            }
           

            var app = App.SingleOrDefault(appId);
            if (app != null)
            {
                app.Version = DateTime.Now.ToString("yyyyMMddHHmmss");
                app.Save();
            }
        }

        public static int DeleteSettingByAppid(int appid)
        {
            return AppSetting.Delete("Delete from AppSetting where appid=" +appid);
        }

        public static bool DeleteAppSettingById(int id)
        {
            return AppSetting.Delete(id) > 0;
        }
    }
}
