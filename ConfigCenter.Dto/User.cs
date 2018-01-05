using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ConfigCenter.Dto
{
    public class AccountDto
    {
        [Display(Name = "用户ID")]
        public int Id { get; set; }

        [Display(Name = "用户名称")]
        public string Name { get; set; }

        [Display(Name = "用户密码")]
        public string Password { get; set; }
        [Display(Name = "角色名称")]
        public RoleEnum RoleId { get; set; }
        [Display(Name = "盐值")]
        public string Salt { get; set; }
    }
    #region /app/create

    [Route("/Account/create", "POST")]
    public class CreateAccount : IReturn<BaseResponse>
    {
        public string Id { get; set; }
    }

    #endregion /app/create
}
