using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.snfilterword.setfilter
    /// </summary>
    public class OpenimSnfilterwordSetfilterRequest : BaseTopRequest<Top.Api.Response.OpenimSnfilterwordSetfilterResponse>
    {
        /// <summary>
        /// 上传者身份
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 过滤原因描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 需要过滤的关键词
        /// </summary>
        public string Filterword { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.snfilterword.setfilter";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("creator", this.Creator);
            parameters.Add("desc", this.Desc);
            parameters.Add("filterword", this.Filterword);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("creator", this.Creator);
            RequestValidator.ValidateRequired("filterword", this.Filterword);
        }

        #endregion
    }
}
