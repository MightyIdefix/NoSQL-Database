using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB3.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAB3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        UserFunctions subscribefFunctions = new UserFunctions();

        [HttpGet]
        public ActionResult<string> Get(string myName, string banName)
        {
            return subscribefFunctions.SubcribeToUser(myName, banName);
        }

        [HttpDelete]
        public ActionResult<string> Delete(string myName, string banName)
        {
            return subscribefFunctions.UnsubcribeToUser(myName, banName);
        }
    }
}