using System;
using System.Net.Mail;
using System.Collections;

namespace Joney.UtilityClassLibrary
{
    public class EmailService
    {
        private readonly string M_smtpService;
        private readonly string M_loginID;
        private readonly string M_password;
        private readonly bool M_enableSSL;
        private readonly int M_port;
        private readonly string emailID = System.Configuration.ConfigurationManager.AppSettings["SysEmailID"];
        private readonly string emailPWD = System.Configuration.ConfigurationManager.AppSettings["SysEmailCode"];
        /// <summary>
        /// 构造邮件通知服务类 实例
        /// </summary>
        /// <param name="smtpService">SMTP服务器的IP地址</param>
        /// <param name="enableSSL">是否使用SSL链接SMTP服务器:SSL加密</param>
        /// <param name="port">SMTP服务器端口</param>
        /// <param name="loginID">用于登录SMTP服务器的用户名</param>
        /// <param name="password">登录密码</param>
        public EmailService(string smtpService, bool enableSSL, int port, string loginID, string password)
        {
            this.M_smtpService = "smtp.qq.com";// smtpService == "" ? "smtp.sina.com" : smtpService;//服务器
            this.M_enableSSL = true;// enableSSL;//是否SSL加密
            this.M_port = 25;// port == 0 ? 587 : port;//端口25//465
            this.M_loginID = emailID;// == "" ? emailID : loginID;//发件人帐号
            this.M_password = DecodeBase64(emailPWD);// == "" ? emailPWD : password;//发件人密匙
        }

        /// <summary>
        /// 发送邮件通知到指定的Email地址:（发件人，收件人地址，抄送，标题，内容）
        /// </summary>
        /// <param name="senderName">发件人名称</param>
        /// <param name="addressee">收件人Email地址</param>
        /// <param name="cctos">抄送</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件</param>
        public bool SendTo(string senderName, string addressee, string cctos, string title, string content, ArrayList files)
        {
            bool result = false;
            try
            {
                MailAddress fromAdd = new MailAddress(this.M_loginID, "系统管理员");
                MailAddress toAdd = new MailAddress(addressee, "用户激活验证");
                MailMessage mail = new MailMessage(fromAdd, toAdd);
                //MailMessage mail = new MailMessage();
                //mail.To.Add(addressee);//收件人地址
                //mail.From = new MailAddress(this.M_loginID, senderName, Encoding.UTF8);//发件人地址、显示名称
                mail.Subject = title;//标题
                mail.SubjectEncoding = System.Text.Encoding.UTF8;//标题编码
                mail.Body = content;//邮件正文内容
                mail.BodyEncoding = System.Text.Encoding.UTF8;// Encoding.UTF8;//内容编码
                mail.IsBodyHtml = true;//正文是否HTML编码
                mail.Priority = MailPriority.Normal;//.Normal;//优先级：High,Low,Normal
                //mail.Attachments.Add(new Attachment("E:\\Mail.doc"));//单个附件
                if (files.Count > 0)
                {
                    string fileName;
                    for (int i = 0; i < files.Count; i++)
                    {
                        fileName = files[i].ToString();
                        mail.Attachments.Add(new System.Net.Mail.Attachment(fileName));//多个附件
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Timeout = 20000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = this.M_smtpService;//指定SMTP服务器
                smtp.EnableSsl = this.M_enableSSL;//是否采用SSL加密
                smtp.Port = this.M_port;//端口号
                smtp.Credentials = new System.Net.NetworkCredential(this.M_loginID, this.M_password);//用户名&密码
                smtp.UseDefaultCredentials = true;

                string[] receiver = System.Text.RegularExpressions.Regex.Split(addressee + cctos, ";");
                if (receiver.Length > 1)
                {
                    for (int i = 0; i < receiver.Length; i++)
                    {
                        try
                        {
                            mail.To.Add(receiver[i].ToString());
                            smtp.Send(mail);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    smtp.Send(mail);
                }

                mail.Dispose();
                //mail.Attachments.Dispose();
                result = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public EmailInfo ReclieveEmail()
        {
            EmailInfo email = new EmailInfo();

            return email;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncryptBase64(string code)
        {
            string res = string.Empty;
            byte[] b = System.Text.Encoding.UTF8.GetBytes(code);
            res = Convert.ToBase64String(b);
            return res;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string DecodeBase64(string code)
        {
            string res = string.Empty;
            byte[] c = Convert.FromBase64String(code);
            res = System.Text.Encoding.UTF8.GetString(c);
            return res;
        }
    }

    public class EmailInfo
    {
        /// <param name="senderName">发件人名称</param>
        /// <param name="addressee">收件人Email地址</param>
        /// <param name="cctos">抄送</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        public string SenderName { get; set; }
        public string Addressee { get; set; }
        public string Cctos { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
