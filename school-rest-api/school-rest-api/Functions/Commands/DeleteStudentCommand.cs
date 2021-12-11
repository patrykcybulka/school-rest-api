using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteStudentCommand : ACommand<DeleteStudentDTO>, IRequest<EmptyObjectResult>
    {
        public DeleteStudentCommand(DeleteStudentDTO model) : base(model)
        {
        }
    }
}
