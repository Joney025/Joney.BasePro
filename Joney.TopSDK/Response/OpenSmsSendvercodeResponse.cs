using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenSmsSendvercodeResponse.
    /// </summary>
    public class OpenSmsSendvercodeResponse : TopResponse
    {
        /// <summary>
        /// 返回结果信息
        /// </summary>
        [XmlElement("result")]
        public Top.Api.Domain.BmcResult Result { get; set; }

    }
}
