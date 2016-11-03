using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimTribeJoinResponse.
    /// </summary>
    public class OpenimTribeJoinResponse : TopResponse
    {
        /// <summary>
        /// 群服务code
        /// </summary>
        [XmlElement("tribe_code")]
        public long TribeCode { get; set; }

    }
}
