using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.ModelsFactory
{
    public class UserRepository
    {
        private static UserInfo[] usersForTest =new[]{
            new UserInfo {
                        ID =1,
                        UserCode = "10001",
                        UserName ="admin",
                        Password="123456",
                        Tel="17876991530",
                        Email="junjieyu@msn.com",
                        Address="深圳市南山区科技园北区郎山路36号",
                        Roles=new[]{"admin","manager","employee"}
                    },
            new UserInfo {
                        ID =2,
                        UserCode = "10002",
                        UserName ="joney",
                        Password="123456",
                        Tel="17876991530",
                        Email="junjieyu@msn.com",
                        Address="深圳市南山区科技园北区郎山路36号",
                        Roles=new[]{ "manager", "employee" }
                    },
            new UserInfo {
                        ID =3,
                        UserCode = "10003",
                        UserName ="tester",
                        Password="123456",
                        Tel="17876991530",
                        Email="junjieyu@msn.com",
                        Address="深圳市南山区科技园北区郎山路36号",
                        Roles=new[]{ "employee", "guest"}
                    },
            new UserInfo {
                        ID =4,
                        UserCode = "10004",
                        UserName ="demo",
                        Password="123456",
                        Tel="17876991530",
                        Email="junjieyu@msn.com",
                        Address="深圳市南山区科技园北区郎山路36号",
                        Roles=new[]{ "employee", "guest"}
                    }
        };

        public UserRepository()
        {
            try
            {
                //for (int i = 0; i < 1000; i++)
                //{
                //    UserInfo user = new UserInfo {
                //        ID = i,
                //        UserCode = "1000"+i,
                //        UserName ="Admin"+i,
                //        Password="123456",
                //        Tel="17876991530",
                //        Email="junjieyu@msn.com",
                //        Address="深圳市南山区科技园北区郎山路"+i+"号",
                //        Roles=new[]{"Admin"}
                //    };
                //    usersForTest[i]= user;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return usersForTest
                .Any(u => u.UserName == userName && u.Password == password);
        }

        public string[] GetRoles(string userName)
        {
            return usersForTest
                .Where(u => u.UserName == userName)
                .Select(u => u.Roles)
                .FirstOrDefault();
        }

        public UserInfo GetByNameAndPassword(string name, string password)
        {
            return usersForTest
                .FirstOrDefault(u => u.UserName == name && u.Password == password);
        }


    }
}
