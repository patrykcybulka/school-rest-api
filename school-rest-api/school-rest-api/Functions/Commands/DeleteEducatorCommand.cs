using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteEducatorCommand : ACommand<DeleteEducatorDTO>, IRequest<EmptyObjectResult>
    {
        public DeleteEducatorCommand(DeleteEducatorDTO model) : base(model)
        {
        }
    }
}
