using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{

    //https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: //https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult Get()
        {
            string[] studentNames = new string[] { "Alice", "Bob", "Charlie", "David", "Eva" };

            return Ok(studentNames);
        }
    }
}
