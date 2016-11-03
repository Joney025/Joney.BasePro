using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// ItemVideo Data Structure.
    /// </summary>
    [Serializable]
    public class ItemVideo : TopObject
    {
        /// <summary>
        /// 创建时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        [XmlElement("created")]
        public string Created { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [XmlElement("item_id")]
        public long ItemId { get; set; }

        /// <summary>
        /// 修改时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        [XmlElement("modified")]
        public string Modified { get; set; }

        /// <summary>
        /// ItemVideo对应的Item的数字id
        /// </summary>
        [XmlElement("num_iid")]
        public long NumIid { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        [XmlElement("video_id")]
        public long VideoId { get; set; }
    }
}
