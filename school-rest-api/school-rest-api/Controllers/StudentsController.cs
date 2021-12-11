using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : AController
    {

        public StudentsController(IMediator mediator, ILogger<StudentsController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var command = new GetAllStudentsQuery();

            return await sendCommand(command);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDTO model)
        {
            var command = new AddStudentCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDTO model)
        {
            var command = new UpdateStudentCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent([FromBody] DeleteStudentDTO model)
        {
            var command = new DeleteStudentCommand(model);

            return await sendCommand(command);
        }
    }
}
