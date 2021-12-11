using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddEducatorCommand : BaseCommand<AddEducatorDTO>, IRequest<AddEducatorResult>
    {
        public AddEducatorCommand(AddEducatorDTO model) : base(model)
        {
        }
    }
}
