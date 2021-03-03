using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB3.DAL;
using DAB3.Models;
using DAB3.Services;
using Microsoft.AspNetCore.Mvc;

namespace DAB3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircleController : ControllerBase
    {
        private readonly CirclesService _circlesService;

        UserFunctions circleFunctions = new UserFunctions();

        public CircleController(CirclesService circlesService)
        {
            _circlesService = circlesService;
        }

        [HttpGet]
        public ActionResult<string> Get(string myName, string otherUserName, string circleName)
        {
            return circleFunctions.AddUserToCircle(myName, otherUserName, circleName);
        }

        [HttpPost]
        public ActionResult<string> Create(string myName, string circleName)
        {
            return circleFunctions.CreateCircle(myName, circleName);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Circle circleIn)
        {
            var circle = _circlesService.Get(id);

            if (circle == null)
            {
                return NotFound();
            }

            _circlesService.Update(id, circleIn);

            return NoContent();
        }

        [HttpDelete]
        public ActionResult<string> Delete(string myName, string otherUserName, string circleName)
        {
            return circleFunctions.RemoveUserFromCircle(myName, otherUserName, circleName);
        }

    }
}

