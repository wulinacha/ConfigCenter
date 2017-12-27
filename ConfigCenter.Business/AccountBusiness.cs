using AutoMapper;
using ConfigCenter.Dto;
using System.Collections.Generic;
using System.Linq;
using ConfigCenterConnection;

namespace ConfigCenter.Business
{
    public class AccountBusiness
    {
        public static List<AccountDto> GetAccounts(int pageIndex, int pageSize, string kword, out long totalPage)
        {
            var page = Account.Page(pageIndex, pageSize, "WHERE AppId LIKE @0", new object[] { "%" + kword + "%" });
            totalPage = page.TotalItems;
            return Mapper.Map<List<Account>, List<AccountDto>>(page.Items);
        }

        public static AccountDto GetAccountById(int Id)
        {
            return Mapper.Map<Account, AccountDto>(Account.SingleOrDefault("WHERE id=@0", Id));
        }
        public static List<AccountDto> GetAccountList()
        {
            return Mapper.Map<List<Account>, List<AccountDto>>(Account.Query("select * from Account",null).ToList());
        }
        public static AccountDto GetAccountByName(string name)
        {
            return Mapper.Map<Account, AccountDto>(Account.SingleOrDefault("WHERE name=@0", name));
        }
        public static AccountDto GetAppVersion(string appIdAndEnv)
        {
            string AppId, Environment;
            SplitAppIdAndEnv(appIdAndEnv, out AppId, out Environment);
            return Mapper.Map<Account, AccountDto>(Account.SingleOrDefault("WHERE AppId=@0 and Environment=@1",
                AppId, Environment));
        }

        private static void SplitAppIdAndEnv(string appIdAndEnv, out string AppId, out string Environment)
        {
            string[] configs = appIdAndEnv.Split('-');
            AppId = configs[0];
            Environment = configs[1];
        }

        public static void SaveUser(AccountDto userDto)
        {
            var user = Mapper.Map<AccountDto, Account>(userDto);
            user.Save();
        }

        public static bool DeleteAccountById(int id)
        {
            return Account.Delete(id) > 0;
        }
    }
}