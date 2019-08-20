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
        protected UserIdentity UserIdentity => new UserIdentity() {UserId = 1, Name = "Drama"};

        protected JsonResult Json(object obj)
        {
            return new JsonResult(obj);
        }
    }
}