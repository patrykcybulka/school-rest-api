using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Enums;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("educators")]
    public class EducatorsController : AController
    {
        public EducatorsController(IMediator mediator, ILogger<EducatorsController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = new GetEducatorByIdDTO { Id = id };

            var command = new GetEducatorByIdQuery(model);

            return await sendCommand(command);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllEducatorsQuery();

            return await sendCommand(command);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEducatorDTO model)
        {
            var command = new AddEducatorCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEducatorDTO model)
        {
            var command = new UpdateEducatorCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteEducatorDTO model)
        {
            var command = new DeleteEducatorCommand(model);

            return await sendCommand(command);
        }
    }
}
