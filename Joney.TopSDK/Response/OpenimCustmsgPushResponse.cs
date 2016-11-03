using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimCustmsgPushResponse.
    /// </summary>
    public class OpenimCustmsgPushResponse : TopResponse
    {
        /// <summary>
        /// 消息id，用于定位问题
        /// </summary>
        [XmlElement("msgid")]
        public long Msgid { get; set; }

    }
}
