using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.open.sms.sendvercode
    /// </summary>
    public class OpenSmsSendvercodeRequest : BaseTopRequest<Top.Api.Response.OpenSmsSendvercodeResponse>
    {
        /// <summary>
        /// 发送验证码请求
        /// </summary>
        public string SendVerCodeRequest { get; set; }

        public SendVerCodeRequestDomain SendVerCodeRequest_ { set { this.SendVerCodeRequest = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.open.sms.sendvercode";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("send_ver_code_request", this.SendVerCodeRequest);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("send_ver_code_request", this.SendVerCodeRequest);
        }

	/// <summary>
	/// SendVerCodeRequestDomain Data Structure.
	/// </summary>
	[Serializable]
	public class SendVerCodeRequestDomain : TopObject
	{
	        /// <summary>
	        /// 短信内容替换上下文
	        /// </summary>
	        [XmlElement("context")]
	        public string Context { get; set; }
	
	        /// <summary>
	        /// 设备id
	        /// </summary>
	        [XmlElement("device_id")]
	        public string DeviceId { get; set; }
	
	        /// <summary>
	        /// 设备级别的发送次数限制
	        /// </summary>
	        [XmlElement("device_limit")]
	        public Nullable<long> DeviceLimit { get; set; }
	
	        /// <summary>
	        /// 发送次数限制的时间，单位为秒，如1个小时内一个设备最多发多少短信
	        /// </summary>
	        [XmlElement("device_limit_in_time")]
	        public Nullable<long> DeviceLimitInTime { get; set; }
	
	        /// <summary>
	        /// 业务域，比如登录的验证码不能用于注册
	        /// </summary>
	        [XmlElement("domain")]
	        public string Domain { get; set; }
	
	        /// <summary>
	        /// 验证码失效时间，单位为秒
	        /// </summary>
	        [XmlElement("expire_time")]
	        public Nullable<long> ExpireTime { get; set; }
	
	        /// <summary>
	        /// 外部的id，发送失败的消息通知会原封不动的带回，用于和已有的状态进行关联
	        /// </summary>
	        [XmlElement("external_id")]
	        public string ExternalId { get; set; }
	
	        /// <summary>
	        /// 手机号
	        /// </summary>
	        [XmlElement("mobile")]
	        public string Mobile { get; set; }
	
	        /// <summary>
	        /// 手机号的次数限制
	        /// </summary>
	        [XmlElement("mobile_limit")]
	        public Nullable<long> MobileLimit { get; set; }
	
	        /// <summary>
	        /// 手机号的次数限制的时间，单位为秒
	        /// </summary>
	        [XmlElement("mobile_limit_in_time")]
	        public Nullable<long> MobileLimitInTime { get; set; }
	
	        /// <summary>
	        /// session id
	        /// </summary>
	        [XmlElement("session_id")]
	        public string SessionId { get; set; }
	
	        /// <summary>
	        /// session级别的发送次数限制
	        /// </summary>
	        [XmlElement("session_limit")]
	        public Nullable<long> SessionLimit { get; set; }
	
	        /// <summary>
	        /// 发送次数限制的时，单位为秒间，单位为秒
	        /// </summary>
	        [XmlElement("session_limit_in_time")]
	        public Nullable<long> SessionLimitInTime { get; set; }
	
	        /// <summary>
	        /// 特殊权限指定签名
	        /// </summary>
	        [XmlElement("signature")]
	        public string Signature { get; set; }
	
	        /// <summary>
	        /// 签名id
	        /// </summary>
	        [XmlElement("signature_id")]
	        public Nullable<long> SignatureId { get; set; }
	
	        /// <summary>
	        /// long型模板id
	        /// </summary>
	        [XmlElement("template_id")]
	        public Nullable<long> TemplateId { get; set; }
	
	        /// <summary>
	        /// 验证码长度大于等于4，小于等于8
	        /// </summary>
	        [XmlElement("ver_code_length")]
	        public Nullable<long> VerCodeLength { get; set; }
	}

        #endregion
    }
}
