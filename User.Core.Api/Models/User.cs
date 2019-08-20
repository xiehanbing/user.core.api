﻿using System.Collections.Generic;

namespace User.Core.Api.Models
{
    public class AppUser
    {
        public AppUser()
        {
            Properties=new List<UserProperty>();
        }
        public int  Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string  Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string  Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string  Title { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string  Phone { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string  Avatar { get; set; }
        /// <summary>
        /// 性别 1 男 0 女
        /// </summary>
        public byte  Gender { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string  Address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string  Tel { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public string  ProvinceId { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string  Province { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public string  CityId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string  City { get; set; }
        /// <summary>
        /// 名片地址
        /// </summary>
        public string  NameCard { get; set; }
        /// <summary>
        /// 用户属性列表
        /// </summary>
        public List<UserProperty> Properties { get; set; }
    }
}