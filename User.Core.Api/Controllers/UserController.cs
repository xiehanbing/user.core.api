using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly ICapPublisher _capPublisher; 

        public UserController(UserContext userContext, ILogger<UserController> logger,
            ICapPublisher capPublisher)
        {
            _userContext = userContext;
            _logger = logger;
            _capPublisher = capPublisher;
        }
        private void RaiseUserprofileChanedEvent(Models.AppUser user)
        {
            if (_userContext.Entry(user).Property(nameof(user.Name)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Title)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Company)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Avatar)).IsModified
            )
            {
                _capPublisher.Publish("finbook.userapi.userprofilechanged",new IntegrationEvent.UserprofileChangedEvent()
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Title = user.Title,
                    Company = user.Company,
                    Avatar = user.Avatar,
                });
            }
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
                .Include(o => o.Properties)
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
            var originProperties = await _userContext.UserProperties.AsNoTracking().Where(o => o.AppUserId == UserIdentity.UserId).ToListAsync();
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

            using (var transction = _userContext.Database.BeginTransaction())
            {
                //发布用户变更消息
                RaiseUserprofileChanedEvent(user);

                _userContext.Users.Update(user);
                await _userContext.SaveChangesAsync();
             

                transction.Commit();
            }
          
            return Json(user);
        }

        /// <summary>
        /// 检查或创建（当手机号不存在是创建用户）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost, Route("check-or-create")]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            Console.WriteLine("phone is :" + phone);
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);
            //TODO 做手机号验证判断
            if (user == null)
            {
                user = new AppUser()
                {
                    Phone = phone
                };
                await _userContext.Users.AddAsync(user);
            }

            await _userContext.SaveChangesAsync();
            return Ok(new
            {
                user.Id,user.Name,user.Company,user.Title,user.Avatar
            });
        }
        /// <summary>
        /// 获取用户标签选项数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            var result = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            return Json(result);
        }
        /// <summary>
        /// 根据手机号查找用户资料
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Route("byPhone")]
        public async Task<IActionResult> Search(string phone)
        {
            return Json(await _userContext.Users.Include(u => u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId));
        }
        /// <summary>
        /// 更新用户标签
        /// </summary>
        /// <param name="tags">标签列表</param>
        /// <returns></returns>
        [HttpPut, Route("tags")]
        public async Task<IActionResult> UpdateUserTags([FromBody]List<string> tags)
        {
            var originTags = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            var newTags = tags.Except(originTags.Select(o => o.Tag)).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(newTags));
            var removeTags = originTags.Where(o => !tags.Contains(o.Tag)).ToList();
            if (newTags.Any())
            {
                await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new UserTag()
                {
                    CreatedTime = DateTime.Now,
                    UserId = UserIdentity.UserId,
                    Tag = t
                }));
            }


            if (removeTags.Any())
            {
                _userContext.UserTags.RemoveRange(removeTags);
            }
            await _userContext.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet,Route("baseinfo/{userId}")]
        public async Task<IActionResult> FindByUserId(int userId)
        {
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return Ok(user);
        }
    }
}
