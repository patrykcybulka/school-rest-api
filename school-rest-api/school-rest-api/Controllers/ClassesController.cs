using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : AController
    {
        public ClassesController(IMediator mediator, ILogger<ClassesController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            var command = new GetAllClassesQuery();

            return await sendCommand(command);
        }


        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] AddClassDTO model)
        {
            var command = new AddClassCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDTO model)
        {
            var command = new UpdateClassCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClass([FromBody] DeleteClassDTO model)
        {
            var command = new DeleteClassCommand(model);

            return await sendCommand(command);
        }
    }
}
