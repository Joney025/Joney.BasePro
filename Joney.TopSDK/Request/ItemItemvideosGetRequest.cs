using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.item.itemvideos.get
    /// </summary>
    public class ItemItemvideosGetRequest : BaseTopRequest<Top.Api.Response.ItemItemvideosGetResponse>
    {
        /// <summary>
        /// 商品id ，传入商品id则不支持分页
        /// </summary>
        public Nullable<long> ItemId { get; set; }

        /// <summary>
        /// 页码。取值范围:大于零的整数; 默认值:1,即返回第一页数据。
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 每页条数。取值范围:大于零的整数;最大值:200;默认值:40。
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 视频id，传入视频id则不支持分页
        /// </summary>
        public Nullable<long> VideoId { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.item.itemvideos.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("item_id", this.ItemId);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("video_id", this.VideoId);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
        }

        #endregion
    }
}
