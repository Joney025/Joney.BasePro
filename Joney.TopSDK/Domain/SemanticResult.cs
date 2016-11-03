using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// SemanticResult Data Structure.
    /// </summary>
    [Serializable]
    public class SemanticResult : TopObject
    {
        /// <summary>
        /// 文本ID
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// 物流包裹 0-包裹正常, 1-包裹有破损, 空-没有包裹信息
        /// </summary>
        [XmlElement("logistics_package")]
        public long LogisticsPackage { get; set; }

        /// <summary>
        /// 物流服务 0-服务好, 1-服务差,  空-没有物流服务信息
        /// </summary>
        [XmlElement("logistics_service")]
        public long LogisticsService { get; set; }

        /// <summary>
        /// 物流速度 0-速度快, 1- 速度慢,  空-没有物流速度信息
        /// </summary>
        [XmlElement("logistics_speed")]
        public long LogisticsSpeed { get; set; }
    }
}
