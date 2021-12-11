using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddStudentCommand : ACommand<AddStudentDTO>, IRequest<AddStudentResult>
    {
        public AddStudentCommand(AddStudentDTO model) : base(model)
        {
        }
    }
}
