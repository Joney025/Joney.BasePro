using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OauthCodeCreateResponse.
    /// </summary>
    public class OauthCodeCreateResponse : TopResponse
    {
        /// <summary>
        /// mock out params
        /// </summary>
        [XmlElement("test")]
        public long Test { get; set; }

    }
}
