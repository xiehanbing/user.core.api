using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Core.Api.Data;
using User.Core.Api.Models;

namespace User.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserContext _userContext;
        private readonly ILogger<UserController> _logger;
        public UserController(UserContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            //throw new Exception("测试错误");
            var user = await _userContext.Users
                  .AsNoTracking()
                .Include(o=>o.Properties)
                  .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");
            }
            return new JsonResult(user);
        }
        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="pathc"></param>
        /// <returns></returns>
        [HttpPatch, Route("")]
        public async Task<IActionResult> Patch(JsonPatchDocument<Models.AppUser> pathc)
        {
            var user = await _userContext.Users
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            pathc.ApplyTo(user);
            foreach (var property in user.Properties)
            {
                _userContext.Entry(property).State = EntityState.Detached;
            }
            var originProperties =await _userContext.UserProperties.AsNoTracking().Where(o=>o.AppUserId==UserIdentity.UserId).ToListAsync();
            var allProperties = originProperties.Union(user.Properties).Distinct();
            var remove = originProperties.Except(user.Properties);
            var newProperties = allProperties.Except(originProperties);

            foreach (var userProperty in remove)
            {
                _userContext.UserProperties.Remove(userProperty);

            }
            foreach (var userProperty in newProperties)
            {
                _userContext.UserProperties.Add(userProperty);

            }

            await _userContext.SaveChangesAsync();
            return Json(user);
        }

        /// <summary>
        /// 检查或创建（当手机号不存在是创建用户）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost,Route("check-or-create")]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            Console.WriteLine("phone is :"+phone);
           var user= await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);
            //TODO 做手机号验证判断
            if (user==null)
            {
                user = new AppUser()
                {
                    Phone = phone
                };
               await _userContext.Users.AddAsync(user);
            }

            return Ok(user.Id);
        }
    }
}
