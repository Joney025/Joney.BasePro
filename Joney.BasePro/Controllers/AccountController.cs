using Joney.ModelsFactory;
using Joney.UtilityClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Joney.BasePro.Controllers
{
    public class AccountController : Controller
    {
        UserRepository UR = new UserRepository();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistUser(FormCollection form)
        {
            try
            {
                var username = string.IsNullOrEmpty(form["UserName"])?"":form["UserName"];
                var password = string.IsNullOrEmpty(form["Password"])?"":form["Password"];
                var password2 = string.IsNullOrEmpty(form["Password2"]) ? "" : form["Password2"];
                var fData = Request.Files["upImg"];
                if (username=="")
                {
                    ModelState.AddModelError("UserName","用户名称不能为空");
                }
                if (password=="")
                {
                    ModelState.AddModelError("Password", "密码不能为空");
                }
                if (password2 == "")
                {
                    ModelState.AddModelError("Password2", "确认密码不能为空");
                }
                if (password != password2)
                {
                    ModelState.AddModelError("MSG", "两次输入密码不一致");
                }
                var imgStream = fData.InputStream;
                byte[] fByte = new byte[fData.ContentLength];
                imgStream.Read(fByte,0,fData.ContentLength);
                imgStream.Seek(0,System.IO.SeekOrigin.Begin);
                var fileExt = ".bmp|.png|.gif|.jpeg|.jpg|.pcx|.tga|.exif|.fpx|.svg|.psd|.cdr|.pcd|.dxf|.ufo|.eps|.ai|.raw";
                var imgB64 = "";
                if (Path.GetExtension(fData.FileName).Contains(fileExt))
                {
                    MemoryStream ms = new MemoryStream(fByte);
                    Image image = Image.FromStream(ms);
                    var imgH = image.Height;
                    var imgW = image.Width;
                    imgB64 =Convert.ToBase64String(fByte,0,fByte.Length);
                }
                ViewData["ImgPreview"]= imgB64;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("MSG",ex.Message.ToString());
            }
            return Register();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogOn(FormCollection form,string ReturnUrl)
        {
            var loginFlag = false;
            try
            {
                if (!string.IsNullOrEmpty(form["UserName"]) && !string.IsNullOrEmpty(form["Password"]))
                {
                    var name = form["UserName"].ToString().Trim().ToLower();
                    var password = form["Password"].ToString().Trim().ToLower();
                    if (UR.ValidateUser(name, password))
                    {
                        UserInfo user = UR.GetByNameAndPassword(name,password);
                        if (user!=null)
                        {
                            var userStr = JsonConvert.SerializeObject(user);//user.Roles.Aggregate((i,j)=>i+","+j)
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1,
                                user.UserName,
                                DateTime.Now,
                                DateTime.Now.Add(FormsAuthentication.Timeout),
                                false,
                                userStr,
                                "/"
                                );
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,FormsAuthentication.Encrypt(ticket));
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);
                            loginFlag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loginFlag = false;
                LogHelper.Jlogger(MethodBase.GetCurrentMethod().DeclaringType, "INFO","登陆失败："+ex.Message.ToString());//log4net日志。
            }
            if (loginFlag)
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("loginMsgValidator", "用户名或密码不正确！");
                return Login(ReturnUrl);
            }
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);//Login("");//RedirectToAction("Index","Home");
        }
    }
}