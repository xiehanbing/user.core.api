using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Dtos;

namespace Project.Api.Controllers
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
                Console.WriteLine("project api user identity id is"+identity.UserId);
                return identity;
            }
        }

        protected JsonResult Json(object obj)
        {
            return new JsonResult(obj);
        }
    }
}