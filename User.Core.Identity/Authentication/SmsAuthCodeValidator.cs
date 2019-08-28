using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Core.Identity.Services;

namespace User.Core.Identity.Authentication
{
    public class SmsAuthCodeValidator:IExtensionGrantValidator
    {
        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidator(IUserService userService, IAuthCodeService authCodeService)
        {
            _userService = userService;
            _authCodeService = authCodeService;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var errorValidationResult=new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //检查验证码
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = errorValidationResult;
                return;
                
            }
            //完成用户注册
            var userId =await _userService.CheckOrCreateAsync(phone);
            if (userId==null)
            {
                context.Result = errorValidationResult;
                return;
            }

            var claims = new[]
            {
                new Claim("name", userId.Name??""),
                new Claim("avatar", userId.Avatar??""),
                new Claim("title", userId.Title??""),
                new Claim("company", userId.Company??"")
            };
            context.Result = new GrantValidationResult(userId.Id.ToString(),GrantType,claims);
        }

        public string GrantType => "sms_auth_code";
    }
}