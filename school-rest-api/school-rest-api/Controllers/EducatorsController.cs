using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Commands;
using school_rest_api.Models;
using school_rest_api.Responses;
using System.Net;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EducatorsController : ControllerBase
    {
        private readonly IMediator                   _mediator;
        private readonly ILogger<EducatorsController> _logger;

        public EducatorsController(IMediator mediator, ILogger<EducatorsController> logger)
        {
            _mediator = mediator;
            _logger   = logger;
        }

        [HttpPost("AddEducator")]
        public async Task<IActionResult> Post([FromBody] AddEducatorDTO model)
        {
            var command = new AddEducatorCommand(model);

            try
            {
                var response = await _mediator.Send(command);

                return StatusCode((int)HttpStatusCode.Created, new AddEducationResponse(response));
            }
            catch (SchoolException exception)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new BaseResponse
                {
                    IsSuccess = false,
                    Errors    = new List<EErrorCode> { exception.ErrorCode } 
                });
            }
        }
    }
}
