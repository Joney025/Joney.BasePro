using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.custmsg.push
    /// </summary>
    public class OpenimCustmsgPushRequest : BaseTopRequest<Top.Api.Response.OpenimCustmsgPushResponse>
    {
        /// <summary>
        /// 自定义消息内容
        /// </summary>
        public string Custmsg { get; set; }

        public CustMsgDomain Custmsg_ { set { this.Custmsg = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.custmsg.push";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("custmsg", this.Custmsg);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("custmsg", this.Custmsg);
        }

	/// <summary>
	/// CustMsgDomain Data Structure.
	/// </summary>
	[Serializable]
	public class CustMsgDomain : TopObject
	{
	        /// <summary>
	        /// apns推送的附带数据。客户端收到apns消息后，可以从apns结构体的"d"字段中取出该内容。aps.size() + apns_param.size() < 200
	        /// </summary>
	        [XmlElement("apns_param")]
	        public string ApnsParam { get; set; }
	
	        /// <summary>
	        /// apns推送时，里面的aps结构体json字符串，aps.alert为必填字段。本字段为可选，若为空，则表示不进行apns推送。aps.size() + apns_param.size() < 200
	        /// </summary>
	        [XmlElement("aps")]
	        public string Aps { get; set; }
	
	        /// <summary>
	        /// 发送的自定义数据，sdk默认无法解析消息，该数据需要客户端自己解析
	        /// </summary>
	        [XmlElement("data")]
	        public string Data { get; set; }
	
	        /// <summary>
	        /// 可以指定发送方的显示昵称，默认为空，自动使用发送方用户id作为nick
	        /// </summary>
	        [XmlElement("from_nick")]
	        public string FromNick { get; set; }
	
	        /// <summary>
	        /// 发送方userid
	        /// </summary>
	        [XmlElement("from_user")]
	        public string FromUser { get; set; }
	
	        /// <summary>
	        /// 表示消息是否在最近会话列表里面展示。如果为1，则消息不在列表展示，可以认为服务端透明的给客户端下法了一个数据，ios的提示任然通过aps字段控制
	        /// </summary>
	        [XmlElement("invisible")]
	        public Nullable<long> Invisible { get; set; }
	
	        /// <summary>
	        /// 客户端最近消息里面显示的消息摘要
	        /// </summary>
	        [XmlElement("summary")]
	        public string Summary { get; set; }
	
	        /// <summary>
	        /// 接收方appkey，不填默认是发送方appkey，如需跨app发送，需要走审批流程，请联系技术答疑
	        /// </summary>
	        [XmlElement("to_appkey")]
	        public string ToAppkey { get; set; }
	
	        /// <summary>
	        /// 接受者userid列表，单次发送用户数小于100
	        /// </summary>
	        [XmlArray("to_users")]
	        [XmlArrayItem("string")]
	        public List<string> ToUsers { get; set; }
	}

        #endregion
    }
}
