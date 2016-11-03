using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Joney.BasePro.Models
{
    public class SysRoleEnum
    {
        /// <summary>
        /// 系统角色权限枚举
        /// </summary>
        public enum RoleEnum
        {
            admin=1,
            manager=2,
            emplooyee=3,
            tester=4,
            guest=5
        }
    }
}