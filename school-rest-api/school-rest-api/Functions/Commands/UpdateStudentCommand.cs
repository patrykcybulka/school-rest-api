using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateStudentCommand : AFunction<UpdateStudentDTO>, IRequest<UpdateStudentResult>
    {
        public UpdateStudentCommand(UpdateStudentDTO model) : base(model)
        {
        }
    }
}
