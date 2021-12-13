using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("classes")]
    public class ClassesController : AController
    {
        public ClassesController(IMediator mediator, ILogger<ClassesController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = new GetClassByIdDTO { Id = id };

            var command = new GetClassByIdQuery(model);

            return await sendCommand(command);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllClassesQuery();

            return await sendCommand(command);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddClassDTO model)
        {
            var command = new AddClassCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClassDTO model)
        {
            var command = new UpdateClassCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteClassDTO model)
        {
            var command = new DeleteClassCommand(model);

            return await sendCommand(command);
        }
    }
}
