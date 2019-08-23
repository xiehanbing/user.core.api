﻿namespace User.Core.Api.Dtos
{
    public class UserIdentity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int  UserId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string  Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string  Avatar { get; set; }

        public string Title
        { get; set; }

        public string Company { get; set; }
    }
}