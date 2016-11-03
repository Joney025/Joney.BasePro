using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.ModelsFactory
{
    public class BaseInfo
    {
        [Key]
        [Display(Name = "ID")]
        public virtual int? ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "用户账号不能为空.")]//,ErrorMessageResourceName ="UserCodeReq")
        [Display(Name = "用户代码")]
        [MinLength(6), MaxLength(20)]
        public virtual string UserCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空.")]
        [Display(Name = "用户名称")]
        [MinLength(6), MaxLength(20)]
        public virtual string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "用户密码不能为空.")]
        [MinLength(6), MaxLength(20)]
        [Display(Name = "密码")]
        public virtual string Password { get; set; }

        [Display(Name = "联系电话")]
        public virtual string Tel { get; set; }

        [Display(Name = "电子邮箱")]
        public virtual string Email { get; set; }

        [Display(Name = "地址")]
        public virtual string Address { get; set; }

        public virtual string[] Roles { get; set; }
    }
}
