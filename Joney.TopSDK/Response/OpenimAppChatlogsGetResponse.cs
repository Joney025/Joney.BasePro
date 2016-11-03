using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimAppChatlogsGetResponse.
    /// </summary>
    public class OpenimAppChatlogsGetResponse : TopResponse
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        [XmlElement("result")]
        public Top.Api.Domain.EsMessageResult Result { get; set; }

    }
}
