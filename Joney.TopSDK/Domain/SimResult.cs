using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// SimResult Data Structure.
    /// </summary>
    [Serializable]
    public class SimResult : TopObject
    {
        /// <summary>
        /// 返回文本处理内容
        /// </summary>
        [XmlElement("top_result")]
        public string TopResult { get; set; }

        /// <summary>
        /// 返回结果为true则运行成功，为false则运行失败
        /// </summary>
        [XmlElement("top_status")]
        public bool TopStatus { get; set; }
    }
}
