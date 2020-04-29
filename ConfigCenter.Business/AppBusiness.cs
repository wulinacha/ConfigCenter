using AutoMapper;
using ConfigCenter.Dto;
using ConfigCenter.Repository.Model;
using System.Collections.Generic;

namespace ConfigCenter.Business
{
    public class AppBusiness
    {
        public static List<AppDto> GetApps(int pageIndex, int pageSize, string kword, out long totalPage)
        {
            var page = App.Page(pageIndex, pageSize, "WHERE AppId LIKE @0", new object[] { "%" + kword + "%" });
            totalPage = page.TotalItems;
            return Mapper.Map<List<App>, List<AppDto>>(page.Items);
        }

        public static AppDto GetAppById(int Id)
        {
            return Mapper.Map<App, AppDto>(App.SingleOrDefault("WHERE id=@0", Id));
        }

        public static AppDto GetAppVersion(string appIdAndEnv)
        {
            string AppId, Environment;
            SplitAppIdAndEnv(appIdAndEnv, out AppId, out Environment);
            return Mapper.Map<App, AppDto>(App.SingleOrDefault("WHERE AppId=@0 and Environment=@1", AppId, Environment));
        }

        private static void SplitAppIdAndEnv(string appIdAndEnv, out string AppId, out string Environment)
        {
            string[] configs = appIdAndEnv.Split('-');
            AppId = configs[0];
            Environment = configs[1];
        }

        public static void SaveApp(AppDto appDto)
        {
            var app = Mapper.Map<AppDto, App>(appDto);
            app.Save();
        }

        public static bool DeleteAppById(int id)
        {
            return App.Delete(id) > 0;
        }
    }
}