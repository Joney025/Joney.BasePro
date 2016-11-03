using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimSnfilterwordSetfilterResponse.
    /// </summary>
    public class OpenimSnfilterwordSetfilterResponse : TopResponse
    {
        /// <summary>
        /// 成功
        /// </summary>
        [XmlElement("errid")]
        public string Errid { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        [XmlElement("errmsg")]
        public string Errmsg { get; set; }

    }
}
