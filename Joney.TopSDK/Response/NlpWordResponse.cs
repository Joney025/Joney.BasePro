using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// NlpWordResponse.
    /// </summary>
    public class NlpWordResponse : TopResponse
    {
        /// <summary>
        /// 返回词法分析的结果
        /// </summary>
        [XmlElement("wordresult")]
        public Top.Api.Domain.WordResult Wordresult { get; set; }

    }
}
