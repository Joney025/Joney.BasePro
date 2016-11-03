using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimTribeGettribeinfoResponse.
    /// </summary>
    public class OpenimTribeGettribeinfoResponse : TopResponse
    {
        /// <summary>
        /// 群信息
        /// </summary>
        [XmlElement("tribe_info")]
        public Top.Api.Domain.TribeInfo TribeInfo { get; set; }

    }
}
