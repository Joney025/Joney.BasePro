using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// NlpFeedbackResponse.
    /// </summary>
    public class NlpFeedbackResponse : TopResponse
    {
        /// <summary>
        /// 用户的反馈是否成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }

    }
}
