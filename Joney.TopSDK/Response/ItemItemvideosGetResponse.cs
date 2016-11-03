using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// ItemItemvideosGetResponse.
    /// </summary>
    public class ItemItemvideosGetResponse : TopResponse
    {
        /// <summary>
        /// 商品和视频关联列表
        /// </summary>
        [XmlArray("item_videos")]
        [XmlArrayItem("item_video")]
        public List<Top.Api.Domain.ItemVideo> ItemVideos { get; set; }

        /// <summary>
        /// 总数（根据卖家查询商品列表的时候有)
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
