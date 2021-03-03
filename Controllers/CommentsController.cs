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
    public class CommentsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<string> Create(string comment, string myName, string postId)
        {
            UserFunctions commentFunctions = new UserFunctions();

            return commentFunctions.CreateComment(comment, myName, postId);
        }

    }
}
