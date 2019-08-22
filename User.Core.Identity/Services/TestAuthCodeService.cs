namespace User.Core.Identity.Services
{
    public class TestAuthCodeService:IAuthCodeService
    {
        public bool Validate(string phone, string code)
        {
            return true;
        }
    }
}