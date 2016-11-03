using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Joney.WebAPI.Models
{
    public class WAUsers
    {
        public WAUsers() {
            this.UID = 1;
            this.Name = Name;
            this.Age = 18;
            this.Birthday = DateTime.Now;
            this.MobliePhone = "17876991530";
            this.Address = "深圳市南山区科技园北区郎山路乌石头路8号天明科技大厦8楼";
            this.remark = "测试用户";
        }

        public int UID { get; set; }

        public int Age { get; set; }

        public byte sex { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string MobliePhone { get; set; }

        public string Address { get; set; }

        public string remark { get; set; }
    }
}