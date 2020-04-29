using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigCenter.Admin;
using ConfigCenter.Admin.Models;
using ConfigCenter.Dto;
using Microsoft.AspNetCore.Http;

namespace ConfigCenter.Admin.Common
{
    public class LoginHelper
    {
        public static void Login(LoginModel login,ISession session)
        {
            session.SetString("name", login.name);
            session.SetString("password", login.password);
            session.SetString("rolename",login.rolename);
        }

        public static void Logout()
        {
            var session = CurrentHttpContext.Current.Session;
            session.Clear();

        }

        public static string GetSaltPassword(string password, string salt)
        {
            //return $"{password}{salt}".ToPassword();
            return (password+salt).ToPassword();
        }

        public static string GetGuidShort() {
            return Guid.NewGuid().ToString("N");
        } 

    }
}