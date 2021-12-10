using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator                   _mediator;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
        {
            _mediator = mediator;
            _logger   = logger;
        }
    }
}
