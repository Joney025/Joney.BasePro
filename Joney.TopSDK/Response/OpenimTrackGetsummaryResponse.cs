using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenimTrackGetsummaryResponse.
    /// </summary>
    public class OpenimTrackGetsummaryResponse : TopResponse
    {
        /// <summary>
        /// 用户最近访问信息
        /// </summary>
        [XmlElement("tracksummary")]
        public Top.Api.Domain.Tracksummary Tracksummary { get; set; }

    }
}
