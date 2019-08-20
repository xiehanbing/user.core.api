using System;
using System.Data;

namespace User.Core.Api.Models
{
    /// <summary>
    /// 文件内容
    /// </summary>
    public class BpFile
    {
        /// <summary>
        /// 标示
        /// </summary>
        public int  Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string  FileName { get; set; }
        /// <summary>
        /// 源文件地址
        /// </summary>
        public string  OriginFilePath { get; set; }
        /// <summary>
        /// 格式转换后的文件地址
        /// </summary>
        public string  FromatFilePath { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}