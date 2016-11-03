using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimChatlogsImportResponse.
    /// </summary>
    public class OpenimChatlogsImportResponse : TopResponse
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("ret")]
        public long Ret { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [XmlElement("succ")]
        public bool Succ { get; set; }

    }
}
