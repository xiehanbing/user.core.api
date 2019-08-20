using System;

namespace User.Core.Api.Models
{
    /// <summary>
    /// 用户标签
    /// </summary>
    public class UserTag
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string  Tag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime  CreatedTime { get; set; }
    }
}