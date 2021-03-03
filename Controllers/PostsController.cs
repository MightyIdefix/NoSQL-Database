using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB3.DAL;
using DAB3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAB3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        UserFunctions postFunctions = new UserFunctions();

        [HttpPost]
        public ActionResult<string> Create(string myName, string content, string circleNamesList, string img)
        {
            List<string> circleList = new List<string>();

            string[] circleSplit = circleNamesList.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var index in circleSplit)
            {
                if (index != null)
                {
                    circleList.Add(index);
                }
            }

            return postFunctions.CreatePost(myName, content, circleList, img);
        }
    }
}