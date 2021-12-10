using Microsoft.AspNetCore.Mvc;
using school_rest_api.Entries;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(ILogger<ClassesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetClasses")]
        public IEnumerable<ClassEntry> Get()
        {
            return null;
        }
    }
}
