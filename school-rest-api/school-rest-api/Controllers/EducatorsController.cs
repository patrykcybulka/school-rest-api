using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Enums;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EducatorsController : AController
    {
        public EducatorsController(IMediator mediator, ILogger<EducatorsController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEducations()
        {
            var command = new GetAllEducatorsQuery();

            return await sendCommand(command);
        }

        [HttpPost]
        public async Task<IActionResult> AddEducator([FromBody] AddEducatorDTO model)
        {
            var command = new AddEducatorCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEducator([FromBody] UpdateEducatorDTO model)
        {
            var command = new UpdateEducatorCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEducator([FromBody] DeleteEducatorDTO model)
        {
            var command = new DeleteEducatorCommand(model);

            return await sendCommand(command);
        }
    }
}
