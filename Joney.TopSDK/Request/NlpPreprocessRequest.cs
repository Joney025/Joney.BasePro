using System;
using System.Xml.Serialization;
using Top.Api.Domain;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.nlp.preprocess
    /// </summary>
    public class NlpPreprocessRequest : BaseTopRequest<Top.Api.Response.NlpPreprocessResponse>
    {
        /// <summary>
        /// 1)繁简字转换：func_type=1，对应type =1 繁转简 type=2 简转繁；2)拆分字转换：func_type =2，对应type=1 文字拆分 type=2 拆分字合并；3)文字转拼音：func_type =3，对应type=1 文字转拼音 type=2 拼音+声调；4)谐音同音字替换：func_type =4，对应type=1 谐音字替换 type=2 同音字替换；5)形似字替换：func_type =5，对应type=1 形似字替换;
        /// </summary>
        public Nullable<long> FuncType { get; set; }

        /// <summary>
        /// 谐音字转换、形似字转换需提供关键词进行替换，关键词之间以#分隔。keyword示例：兼职#招聘#微信、天猫#日结#微信#招聘#加微
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }

        public Text Text_ { set { this.Text = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.nlp.preprocess";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("func_type", this.FuncType);
            parameters.Add("keyword", this.Keyword);
            parameters.Add("text", this.Text);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("func_type", this.FuncType);
            RequestValidator.ValidateRequired("text", this.Text);
        }

        #endregion
    }
}
