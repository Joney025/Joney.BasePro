using System;
using System.Xml.Serialization;
using Top.Api.Domain;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.nlp.semantic.texts.analyze
    /// </summary>
    public class NlpSemanticTextsAnalyzeRequest : BaseTopRequest<Top.Api.Response.NlpSemanticTextsAnalyzeResponse>
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Texts { get; set; }

        public List<Text> Texts_ { set { this.Texts = TopUtils.ObjectToJson(value); } } 

        /// <summary>
        /// 需要获取的语义分析结果类型，以半角逗号(,)分隔,可以指定获取不同类型值的结果,以(=)号分割。如logistics_speed=0,logistics_speed=1;logistics_speed-物流速度分析;logistics_service-物流服务态度分析;logistics_package-物流包裹破损分析;
        /// </summary>
        public string Types { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.nlp.semantic.texts.analyze";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("texts", this.Texts);
            parameters.Add("types", this.Types);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("texts", this.Texts);
            RequestValidator.ValidateObjectMaxListSize("texts", this.Texts, 50);
        }

        #endregion
    }
}
