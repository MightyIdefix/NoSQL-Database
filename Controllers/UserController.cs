using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB3.Models;
using DAB3.Services;
using Microsoft.AspNetCore.Mvc;

namespace DAB3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UsersService _usersService;
        List<Users> Users = new List<Users>();
        

       public UserController(UsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet]          //Done
        public ActionResult<string> Get(string name)
        {
            var user = _usersService.FindUserFromName(name);

            if (user == null)
            {
                return NotFound();
            }

            return "Welcome " + user[0].UserName;
        }

        [HttpPost]
        public ActionResult<string> Create(string name, string gender, string age)
        {

            int i = 0;
            if (!Int32.TryParse(age, out i))
            {
                i = -1;
            }

            Users user = new Users
            {
                UserName = name,
                Age = i,
                Gender = gender,

            };
            Circle circle = new Circle {CircleName = "Public", UserIds = new List<string>()};
            circle.CircleOwner = circle.Id;
            circle.UserIds.Add(user.Id);
            CirclesService service = new CirclesService();
            service.Create(circle);
            user.MyCirclesId.Add(circle.Id);
            _usersService.Create(user);

            return "User created";
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Users userIn)          //Done
        {
            var user = _usersService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _usersService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]           //Done
        public IActionResult Delete(string id)
        {
            var user = _usersService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _usersService.Remove(user.Id);

            return NoContent();
        }
    }
}
