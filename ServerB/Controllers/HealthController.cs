using Microsoft.AspNetCore.Mvc;

namespace ServerB.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class HealthController:ControllerBase
{
    [HttpGet("/healthCheck")]
    public IActionResult check() => Ok("ok");

    [HttpGet("/getPerson")]
    public async Task<List<Person>> getPerson(int id) {
        var p1 = new Person("faith", 24, "man");
        var p2 = new Person("evil", 25, "woman");
        var p3 = new Person("won", 27, "man");
        List<Person> persons = new List<Person>();
        persons.Add(p1);
        persons.Add(p2);
        persons.Add(p3);
        return persons;
    }
}