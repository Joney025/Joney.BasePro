using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// BaichuanUserLoginbytokenResponse.
    /// </summary>
    public class BaichuanUserLoginbytokenResponse : TopResponse
    {
        /// <summary>
        /// name
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

    }
}
