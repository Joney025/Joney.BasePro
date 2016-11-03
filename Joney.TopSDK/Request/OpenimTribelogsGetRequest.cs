using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.tribelogs.get
    /// </summary>
    public class OpenimTribelogsGetRequest : BaseTopRequest<Top.Api.Response.OpenimTribelogsGetResponse>
    {
        /// <summary>
        /// 查询起始时间，UTC秒数。必须在一个月内。
        /// </summary>
        public Nullable<long> Begin { get; set; }

        /// <summary>
        /// 查询条数
        /// </summary>
        public Nullable<long> Count { get; set; }

        /// <summary>
        /// 查询结束时间，UTC秒数。必须大于起始时间并小于当前时间
        /// </summary>
        public Nullable<long> End { get; set; }

        /// <summary>
        /// 迭代key
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// 群号
        /// </summary>
        public string TribeId { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.tribelogs.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("begin", this.Begin);
            parameters.Add("count", this.Count);
            parameters.Add("end", this.End);
            parameters.Add("next", this.Next);
            parameters.Add("tribe_id", this.TribeId);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("begin", this.Begin);
            RequestValidator.ValidateRequired("count", this.Count);
            RequestValidator.ValidateRequired("end", this.End);
            RequestValidator.ValidateRequired("tribe_id", this.TribeId);
        }

        #endregion
    }
}
