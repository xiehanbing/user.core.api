
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using User.Core.Api.Controllers;
using User.Core.Api.Data;
using User.Core.Api.Models;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
namespace User.Api.UnitTest
{
    public class UserControllerUnitTest
    {
        private UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new UserContext(options);

            context.Users.Add(new AppUser()
            {
                Id = 1,
                Name = "Drama"
            });
            context.SaveChanges();
            return context;
        }

        private (UserController user,UserContext context) GetUserController()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            var cap=new Mock<ICapPublisher>().Object;
            var controller = new UserController(context, logger, cap);
            return (controller,context);
        }
        [Fact]
        public async Task Get_ReturnUser_WithExpectedParamters()
        {
           
            var controller = GetUserController().user;
            var response = await controller.Get();
            Console.WriteLine(response);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("Drama");
            //Assert.IsType<JsonResult>(response);
        }
        [Fact]
        public async Task Patch_ReturnNewName_WithExpectedParamters()
        {
            var userConfig = GetUserController();
            var controller = userConfig.user;
            var document=new JsonPatchDocument<AppUser>();

           // document.Add(user => user.Name, "drama");

            document.Replace(u => u.Name, "drama");
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("drama");

            //assert name value in ef context
            var dbContext = userConfig.context;
            var contextValue = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            contextValue.Should().NotBeNull();
            appUser.Name.Should().Be(contextValue.Name);
            contextValue.Name.Should().Be("drama");
        }

        [Fact]
        public async Task Patch_ReturnNewProperties_WithAddNewProperties()
        {
            var userConfig = GetUserController();
            var controller = userConfig.user;
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties,new List<UserProperty>()
            {
                new UserProperty(){Key = "fin_industry",Value = "»¥ÁªÍø",Text = "»¥ÁªÍø"}
            });
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.First().Value.Should().Be("»¥ÁªÍø");
            appUser.Properties.First().Key.Should().Be("fin_industry");

            //assert name value in ef context
            var dbContext = userConfig.context;
            var contextValue = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            contextValue.Properties.Should().NotBeNull();
            contextValue.Properties.First().Value.Should().Be("»¥ÁªÍø");
            contextValue.Properties.First().Key.Should().Be("fin_industry");
        }


        [Fact]
        public async Task Patch_ReturnNewProperties_WithRemoveProperties()
        {
            var userConfig = GetUserController();
            var controller = userConfig.user;
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>());
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();


            //assert name value in ef context
            var dbContext = userConfig.context;
            var contextValue = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            contextValue.Properties.Should().BeEmpty();
        }
    }
}
