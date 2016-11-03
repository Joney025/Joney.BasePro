using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.userservice.get
    /// </summary>
    public class OpenimUserserviceGetRequest : BaseTopRequest<Top.Api.Response.OpenimUserserviceGetResponse>
    {
        /// <summary>
        /// 某一天
        /// </summary>
        public Nullable<DateTime> Date { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 页面记录数
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.userservice.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("date", this.Date);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("date", this.Date);
        }

        #endregion
    }
}
