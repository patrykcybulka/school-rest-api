using MediatR;
using school_rest_api.Models.DTO;

namespace school_rest_api.Functions.Commands
{
    public class UpdateStudentCommand : BaseCommand<UpdateStudentDTO>, IRequest<UpdateStudentResult>
    {
        public UpdateStudentCommand(UpdateStudentDTO model) : base(model)
        {
        }
    }
}
