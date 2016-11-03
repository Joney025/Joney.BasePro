using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// SendMessageResult Data Structure.
    /// </summary>
    [Serializable]
    public class SendMessageResult : TopObject
    {
        /// <summary>
        /// 延迟发送任务的唯一号
        /// </summary>
        [XmlElement("delay_task_id")]
        public string DelayTaskId { get; set; }

        /// <summary>
        /// 短信条数
        /// </summary>
        [XmlElement("sms_size")]
        public long SmsSize { get; set; }

        /// <summary>
        /// 发送的唯一号
        /// </summary>
        [XmlElement("task_id")]
        public long TaskId { get; set; }
    }
}
