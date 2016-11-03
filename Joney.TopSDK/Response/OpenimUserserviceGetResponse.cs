using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimUserserviceGetResponse.
    /// </summary>
    public class OpenimUserserviceGetResponse : TopResponse
    {
        /// <summary>
        /// 根据站点名称查询产品
        /// </summary>
        [XmlElement("result")]
        public ApiResultDomain Result { get; set; }

	/// <summary>
	/// ApiResultDomain Data Structure.
	/// </summary>
	[Serializable]
	public class ApiResultDomain : TopObject
	{
	        /// <summary>
	        /// 执行成功
	        /// </summary>
	        [XmlElement("code")]
	        public long Code { get; set; }
	
	        /// <summary>
	        /// 执行成功后的结果<br> startTime: 聊天开始时间，现在去敏感数据，精确到天<br> eserviceId：客服nick<br> area：客户所在区域<br> eserviceSendMsgNum：客服发送消息数<br> userSource：用户来源，比如ios, android<br> userSendMsgNum：用户发送消息数<br> channel：客户来源渠道<br> loadingPage：load页面<br> openId：客户nick
	        /// </summary>
	        [XmlElement("data")]
	        public string Data { get; set; }
	
	        /// <summary>
	        /// 错误信息
	        /// </summary>
	        [XmlElement("error_msg")]
	        public string ErrorMsg { get; set; }
	}

    }
}
