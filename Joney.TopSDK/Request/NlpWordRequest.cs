using System;
using System.Xml.Serialization;
using Top.Api.Domain;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.nlp.word
    /// </summary>
    public class NlpWordRequest : BaseTopRequest<Top.Api.Response.NlpWordResponse>
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }

        public Text Text_ { set { this.Text = TopUtils.ObjectToJson(value); } } 

        /// <summary>
        /// 功能类型选择：1)wType=1时提供分词功能，type=0时为基本粒度，type=1时为混合粒度，type=3时为基本粒度和混合粒度共同输出；
        /// </summary>
        public Nullable<long> WType { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.nlp.word";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("text", this.Text);
            parameters.Add("w_type", this.WType);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("text", this.Text);
            RequestValidator.ValidateRequired("w_type", this.WType);
        }

        #endregion
    }
}
