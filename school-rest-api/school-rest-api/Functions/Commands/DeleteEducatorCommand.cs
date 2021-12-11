using MediatR;
using school_rest_api.Models.DTO;

namespace school_rest_api.Functions.Commands
{
    public class DeleteEducatorCommand : BaseCommand<DeleteEducatorDTO>, IRequest<EmptyObjectResult>
    {
        public DeleteEducatorCommand(DeleteEducatorDTO model) : base(model)
        {
        }
    }
}
