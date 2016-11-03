using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// Text Data Structure.
    /// </summary>
    [Serializable]
    public class Text : TopObject
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        [XmlElement("content")]
        public string Content { get; set; }

        /// <summary>
        /// 业务处理ID
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// 文本类型1-评论 2-订单留言 9-其他
        /// </summary>
        [XmlElement("type")]
        public Nullable<long> Type { get; set; }
    }
}
