using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.flash.picture.delete
    /// </summary>
    public class FlashPictureDeleteRequest : BaseTopRequest<Top.Api.Response.FlashPictureDeleteResponse>
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 图片ID字符串,可以一个也可以一组,用英文逗号间隔,如450,120,155
        /// </summary>
        public string PictureIds { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.flash.picture.delete";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("nick", this.Nick);
            parameters.Add("picture_ids", this.PictureIds);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("nick", this.Nick);
            RequestValidator.ValidateRequired("picture_ids", this.PictureIds);
        }

        #endregion
    }
}
