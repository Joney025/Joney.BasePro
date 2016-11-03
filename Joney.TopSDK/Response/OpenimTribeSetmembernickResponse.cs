using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimTribeSetmembernickResponse.
    /// </summary>
    public class OpenimTribeSetmembernickResponse : TopResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [XmlElement("tribe_code")]
        public long TribeCode { get; set; }

    }
}
