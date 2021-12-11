using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteClassCommand : ACommand<DeleteClassDTO>, IRequest<EmptyObjectResult>
    {
        public DeleteClassCommand(DeleteClassDTO model) : base(model)
        {
        }
    }
}
