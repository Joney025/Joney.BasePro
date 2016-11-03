using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: alibaba.dtdream.test.delete
    /// </summary>
    public class AlibabaDtdreamTestDeleteRequest : BaseTopRequest<Top.Api.Response.AlibabaDtdreamTestDeleteResponse>
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "alibaba.dtdream.test.delete";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("id", this.Id);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
        }

        #endregion
    }
}
