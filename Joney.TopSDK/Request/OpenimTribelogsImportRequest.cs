using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.tribelogs.import
    /// </summary>
    public class OpenimTribelogsImportRequest : BaseTopRequest<Top.Api.Response.OpenimTribelogsImportResponse>
    {
        /// <summary>
        /// 消息列表
        /// </summary>
        public string Messages { get; set; }

        public List<TribeTextMessageDomain> Messages_ { set { this.Messages = TopUtils.ObjectToJson(value); } } 

        /// <summary>
        /// 群号。必须为已存在的群，且群主属于本app
        /// </summary>
        public Nullable<long> TribeId { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.tribelogs.import";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("messages", this.Messages);
            parameters.Add("tribe_id", this.TribeId);
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
            RequestValidator.ValidateRequired("tribe_id", this.TribeId);
        }

	/// <summary>
	/// TribeTextMessageDomain Data Structure.
	/// </summary>
	[Serializable]
	public class TribeTextMessageDomain : TopObject
	{
	        /// <summary>
	        /// 发送方userid。必须为本app已导入的账号
	        /// </summary>
	        [XmlElement("from_id")]
	        public string FromId { get; set; }
	
	        /// <summary>
	        /// 消息
	        /// </summary>
	        [XmlElement("message")]
	        public string Message { get; set; }
	
	        /// <summary>
	        /// 消息时间。UTC时间，精确到秒。时间范围必须在当前时间30天内
	        /// </summary>
	        [XmlElement("time")]
	        public Nullable<long> Time { get; set; }
	}

        #endregion
    }
}
