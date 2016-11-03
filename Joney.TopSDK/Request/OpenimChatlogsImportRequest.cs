using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.chatlogs.import
    /// </summary>
    public class OpenimChatlogsImportRequest : BaseTopRequest<Top.Api.Response.OpenimChatlogsImportResponse>
    {
        /// <summary>
        /// 消息序列
        /// </summary>
        public string Messages { get; set; }

        public List<TextMessageDomain> Messages_ { set { this.Messages = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.chatlogs.import";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("messages", this.Messages);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("messages", this.Messages);
            RequestValidator.ValidateObjectMaxListSize("messages", this.Messages, 20);
        }

	/// <summary>
	/// TextMessageDomain Data Structure.
	/// </summary>
	[Serializable]
	public class TextMessageDomain : TopObject
	{
	        /// <summary>
	        /// 发送方userid
	        /// </summary>
	        [XmlElement("from_id")]
	        public string FromId { get; set; }
	
	        /// <summary>
	        /// 消息
	        /// </summary>
	        [XmlElement("message")]
	        public string Message { get; set; }
	
	        /// <summary>
	        /// 消息时间。UTC时间，精确到秒。必须在一个月内
	        /// </summary>
	        [XmlElement("time")]
	        public Nullable<long> Time { get; set; }
	
	        /// <summary>
	        /// 接受方userid
	        /// </summary>
	        [XmlElement("to_id")]
	        public string ToId { get; set; }
	}

        #endregion
    }
}
