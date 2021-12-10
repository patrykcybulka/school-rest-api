using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator                  _mediator;
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(IMediator mediator, ILogger<ClassesController> logger)
        {
            _mediator = mediator;
            _logger   = logger;
        }
    }
}
