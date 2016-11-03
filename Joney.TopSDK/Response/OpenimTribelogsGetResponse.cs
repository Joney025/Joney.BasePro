using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimTribelogsGetResponse.
    /// </summary>
    public class OpenimTribelogsGetResponse : TopResponse
    {
        /// <summary>
        /// 返回结构
        /// </summary>
        [XmlElement("data")]
        public TribeMessageResultDomain Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        [XmlElement("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("retCode")]
        public long RetCode { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [XmlElement("succ")]
        public bool Succ { get; set; }

	/// <summary>
	/// MessageItemDomain Data Structure.
	/// </summary>
	[Serializable]
	public class MessageItemDomain : TopObject
	{
	        /// <summary>
	        /// 节点类型
	        /// </summary>
	        [XmlElement("type")]
	        public string Type { get; set; }
	
	        /// <summary>
	        /// 节点值
	        /// </summary>
	        [XmlElement("value")]
	        public string Value { get; set; }
	}

	/// <summary>
	/// UserDomain Data Structure.
	/// </summary>
	[Serializable]
	public class UserDomain : TopObject
	{
	        /// <summary>
	        /// 发送方所属app
	        /// </summary>
	        [XmlElement("app_key")]
	        public string AppKey { get; set; }
	
	        /// <summary>
	        /// 是否为淘宝账号
	        /// </summary>
	        [XmlElement("taobao_account")]
	        public bool TaobaoAccount { get; set; }
	
	        /// <summary>
	        /// userid
	        /// </summary>
	        [XmlElement("uid")]
	        public string Uid { get; set; }
	}

	/// <summary>
	/// TribeMessageDomain Data Structure.
	/// </summary>
	[Serializable]
	public class TribeMessageDomain : TopObject
	{
	        /// <summary>
	        /// 消息内容节点序列
	        /// </summary>
	        [XmlArray("content")]
	        [XmlArrayItem("message_item")]
	        public List<MessageItemDomain> Content { get; set; }
	
	        /// <summary>
	        /// 发送方
	        /// </summary>
	        [XmlElement("from_id")]
	        public UserDomain FromId { get; set; }
	
	        /// <summary>
	        /// 消息时间。UTC时间
	        /// </summary>
	        [XmlElement("time")]
	        public long Time { get; set; }
	
	        /// <summary>
	        /// 消息类型
	        /// </summary>
	        [XmlElement("type")]
	        public long Type { get; set; }
	
	        /// <summary>
	        /// 消息UUID
	        /// </summary>
	        [XmlElement("uuid")]
	        public long Uuid { get; set; }
	}

	/// <summary>
	/// TribeMessageResultDomain Data Structure.
	/// </summary>
	[Serializable]
	public class TribeMessageResultDomain : TopObject
	{
	        /// <summary>
	        /// 消息列表
	        /// </summary>
	        [XmlArray("messages")]
	        [XmlArrayItem("tribe_message")]
	        public List<TribeMessageDomain> Messages { get; set; }
	
	        /// <summary>
	        /// 迭代key
	        /// </summary>
	        [XmlElement("next_key")]
	        public string NextKey { get; set; }
	}

    }
}
