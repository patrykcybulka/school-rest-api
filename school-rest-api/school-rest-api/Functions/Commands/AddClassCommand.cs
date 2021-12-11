using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddClassCommand : ACommand<AddClassDTO>, IRequest<AddClassResult>
    {
        public AddClassCommand(AddClassDTO model) : base(model)
        {
        }
    }
}
