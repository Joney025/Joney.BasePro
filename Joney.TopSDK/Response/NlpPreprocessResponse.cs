using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// NlpPreprocessResponse.
    /// </summary>
    public class NlpPreprocessResponse : TopResponse
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlElement("processresult")]
        public Top.Api.Domain.ProcessResult Processresult { get; set; }

    }
}
