using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.nlp.similarity
    /// </summary>
    public class NlpSimilarityRequest : BaseTopRequest<Top.Api.Response.NlpSimilarityResponse>
    {
        /// <summary>
        /// 多文本内容
        /// </summary>
        public string Texts { get; set; }

        public TextsDomain Texts_ { set { this.Texts = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.nlp.similarity";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("texts", this.Texts);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("texts", this.Texts);
        }

	/// <summary>
	/// TextsDomain Data Structure.
	/// </summary>
	[Serializable]
	public class TextsDomain : TopObject
	{
	        /// <summary>
	        /// 文本相似度匹配文本内容模板
	        /// </summary>
	        [XmlElement("content_dst")]
	        public string ContentDst { get; set; }
	
	        /// <summary>
	        /// 文本相似度匹配文本
	        /// </summary>
	        [XmlElement("content_src")]
	        public string ContentSrc { get; set; }
	
	        /// <summary>
	        /// 业务处理ID
	        /// </summary>
	        [XmlElement("id")]
	        public string Id { get; set; }
	
	        /// <summary>
	        /// 文本相似度匹配类型：1为余弦距离，2为编辑距离，3为simHash距离
	        /// </summary>
	        [XmlElement("type")]
	        public Nullable<long> Type { get; set; }
	}

        #endregion
    }
}
