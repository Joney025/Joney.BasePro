using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// MediaCategoryUpdateResponse.
    /// </summary>
    public class MediaCategoryUpdateResponse : TopResponse
    {
        /// <summary>
        /// 更新是否成功标志
        /// </summary>
        [XmlElement("success")]
        public bool Success { get; set; }

    }
}
