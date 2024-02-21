using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet("/index")]
        public string test() {
            Console.WriteLine("1111");
            return "hello world";
        }
    }
}
