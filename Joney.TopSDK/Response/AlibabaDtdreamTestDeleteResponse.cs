using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// AlibabaDtdreamTestDeleteResponse.
    /// </summary>
    public class AlibabaDtdreamTestDeleteResponse : TopResponse
    {
        /// <summary>
        /// id
        /// </summary>
        [XmlElement("ret_id")]
        public string RetId { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [XmlElement("ret_msg")]
        public string RetMsg { get; set; }

    }
}
