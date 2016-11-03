using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// NlpSimilarityResponse.
    /// </summary>
    public class NlpSimilarityResponse : TopResponse
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlElement("simresult")]
        public Top.Api.Domain.SimResult Simresult { get; set; }

    }
}
