using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openim.tribe.setmembernick
    /// </summary>
    public class OpenimTribeSetmembernickRequest : BaseTopRequest<Top.Api.Response.OpenimTribeSetmembernickResponse>
    {
        /// <summary>
        /// 被设置昵称的群成员
        /// </summary>
        public string Member { get; set; }

        public UserDomain Member_ { set { this.Member = TopUtils.ObjectToJson(value); } } 

        /// <summary>
        /// 设置的昵称
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 群id
        /// </summary>
        public Nullable<long> TribeId { get; set; }

        /// <summary>
        /// 发起设置昵称的操作者，如果是设置其他成员的昵称，只有普通组的群主和管理员有权限
        /// </summary>
        public string User { get; set; }

        public UserDomain User_ { set { this.User = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openim.tribe.setmembernick";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("member", this.Member);
            parameters.Add("nick", this.Nick);
            parameters.Add("tribe_id", this.TribeId);
            parameters.Add("user", this.User);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("member", this.Member);
            RequestValidator.ValidateRequired("nick", this.Nick);
            RequestValidator.ValidateRequired("tribe_id", this.TribeId);
            RequestValidator.ValidateRequired("user", this.User);
        }

	/// <summary>
	/// UserDomain Data Structure.
	/// </summary>
	[Serializable]
	public class UserDomain : TopObject
	{
	        /// <summary>
	        /// 账户appkey
	        /// </summary>
	        [XmlElement("app_key")]
	        public string AppKey { get; set; }
	
	        /// <summary>
	        /// 是否为淘宝账号
	        /// </summary>
	        [XmlElement("taobao_account")]
	        public Nullable<bool> TaobaoAccount { get; set; }
	
	        /// <summary>
	        /// 用户id
	        /// </summary>
	        [XmlElement("uid")]
	        public string Uid { get; set; }
	}

        #endregion
    }
}
