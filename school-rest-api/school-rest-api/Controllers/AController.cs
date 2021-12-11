using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Exceptions;
using school_rest_api.Responses;
using System.Net;

namespace school_rest_api.Controllers
{
    public abstract class AController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger   _logger;

        public AController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected async Task<IActionResult> sendCommand<T>(IRequest<T> command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return StatusCode((int)HttpStatusCode.Created, new Response(result));
            }
            catch (SchoolException exception)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new Response(exception.ErrorCode));
            }
        }
    }
}
