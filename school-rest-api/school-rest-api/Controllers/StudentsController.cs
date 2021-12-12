using MediatR;
using Microsoft.AspNetCore.Mvc;
using school_rest_api.Enums;
using school_rest_api.Functions.Commands;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;

namespace school_rest_api.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : AController
    {

        public StudentsController(IMediator mediator, ILogger<StudentsController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = new GetStudentByIdQueryDTO { Id = id };

            var command = new GetStudentByIdQuery(model);

            return await sendCommand(command);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllStudentsQuery();

            return await sendCommand(command);
        }

        [HttpGet("by-class-and-group")]
        public async Task<IActionResult> GetByClassAndGroup(Guid id, ELanguageGroup group)
        {
            var model = new GetStudentsByClassAndGroupQueryDTO { Id = id, LanguageGroup = group };

            var command = new GetStudentsByClassAndGroupQuery(model);

            return await sendCommand(command);
        }

        [HttpGet("by-class-and-sorted-by-gender")]
        public async Task<IActionResult> GetByClassAndSortedByGender(Guid id, EGender gender)
        {
            var model = new GetStudentsByClassAndSortedByGenderQueryDTO { Id = id, Gender = gender };

            var command = new GetStudentsByClassAndSortedByGenderQuery(model);

            return await sendCommand(command);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddStudentDTO model)
        {
            var command = new AddStudentCommand(model);

            return await sendCommand(command);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStudentDTO model)
        {
            var command = new UpdateStudentCommand(model);

            return await sendCommand(command);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentDTO model)
        {
            var command = new DeleteStudentCommand(model);

            return await sendCommand(command);
        }
    }
}
