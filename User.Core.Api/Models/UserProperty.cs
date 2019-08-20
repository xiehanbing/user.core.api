using System;

namespace User.Core.Api.Models
{
    /// <summary>
    /// 用户属性
    /// </summary>
    public class UserProperty
    {
        private int? _requestedHashCode;
        /// <summary>
        /// 用户id
        /// </summary>
        public int AppUserId { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 属性文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 重写 相等 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is UserProperty))
                return false;
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            UserProperty item = (UserProperty) obj;
            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }

            return item.Key == this.Key && item.Value == this.Value;
        }

        protected bool Equals(UserProperty other)
        {
            return AppUserId == other.AppUserId && string.Equals(Key, other.Key) && string.Equals(Text, other.Text) && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = (this.Key + this.Value).GetHashCode() ^ 31;

                }

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public bool IsTransient()
        {
            return string.IsNullOrEmpty(this.Key) || string.IsNullOrEmpty(this.Value);
        }

        public static bool operator ==(UserProperty left, UserProperty right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null));
            return left.Equals(right);
        }

        public static bool operator !=(UserProperty left, UserProperty right)
        {
            return !(left == right);
        }
    }
}