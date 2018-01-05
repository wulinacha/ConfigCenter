using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigCenter.Admin;
using ConfigCenter.Dto;

namespace ConfigCenter.Admin.Common
{
    public class LoginHelper
    {
        public static void Login(string name,string password,string roleName,string salt=null)
        {
            var session= HttpContext.Current.Session;
            session["name"] = name;
            session["password"] = password;
            session["roleName"] = roleName;
        }

        public static void Logout()
        {
            var session = HttpContext.Current.Session;
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