using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.media.category.update
    /// </summary>
    public class MediaCategoryUpdateRequest : BaseTopRequest<Top.Api.Response.MediaCategoryUpdateResponse>
    {
        /// <summary>
        /// 文件分类ID,不能为空
        /// </summary>
        public Nullable<long> MediaCategoryId { get; set; }

        /// <summary>
        /// 文件分类名，最大长度20字符，中英文都算一字符,不能为空
        /// </summary>
        public string MediaCategoryName { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.media.category.update";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("media_category_id", this.MediaCategoryId);
            parameters.Add("media_category_name", this.MediaCategoryName);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("media_category_id", this.MediaCategoryId);
            RequestValidator.ValidateRequired("media_category_name", this.MediaCategoryName);
        }

        #endregion
    }
}
