using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;

namespace PersonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<PersonModel> Get()
        {
            var rnd = new Random();
            var person = new PersonModel
            {
                Name = "John Smith",
                Age = rnd.Next(19, 100)
            };
            return Ok(person);
        }
    }
}
