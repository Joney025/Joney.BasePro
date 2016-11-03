using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.nlp.feedback
    /// </summary>
    public class NlpFeedbackRequest : BaseTopRequest<Top.Api.Response.NlpFeedbackResponse>
    {
        /// <summary>
        /// api接口名称
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 反馈的具体原因
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 反馈类型 1-物流信息判断错误
        /// </summary>
        public Nullable<long> Type { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.nlp.feedback";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("api_name", this.ApiName);
            parameters.Add("content", this.Content);
            parameters.Add("description", this.Description);
            parameters.Add("type", this.Type);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("content", this.Content);
        }

        #endregion
    }
}
