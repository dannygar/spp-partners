using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AADController : Controller
    {
        [Authorize]
        // GET: api/aad
        [HttpGet("secure")]
        public IEnumerable<string> GetAccess()
        {
            return new string[] { "Access granted!" };
        }


        // GET: api/aad/test
        [HttpGet("test")]
        public IEnumerable<string> GetTestResponse()
        {
            return new string[] { "Unsecured access is OK" };
        }

    }
}
