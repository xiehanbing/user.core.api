using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Core.Api.Dtos;

namespace User.Core.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity
                {
                    UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value),
                    Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                    Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar")?.Value,
                    Title = User.Claims.FirstOrDefault(c => c.Type == "title")?.Value,
                    Company = User.Claims.FirstOrDefault(c => c.Type == "company")?.Value
                };
                Console.WriteLine("contact api user identity id is:" + identity.UserId);
                return identity;
            }
        }

        protected JsonResult Json(object obj)
        {
            return new JsonResult(obj);
        }
    }
}