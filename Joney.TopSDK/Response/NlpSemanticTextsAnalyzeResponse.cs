using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// NlpSemanticTextsAnalyzeResponse.
    /// </summary>
    public class NlpSemanticTextsAnalyzeResponse : TopResponse
    {
        /// <summary>
        /// 文本语义分析结果
        /// </summary>
        [XmlArray("semantic_list")]
        [XmlArrayItem("semantic_result")]
        public List<Top.Api.Domain.SemanticResult> SemanticList { get; set; }

    }
}
