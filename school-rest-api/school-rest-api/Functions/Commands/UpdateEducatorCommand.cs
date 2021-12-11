using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateEducatorCommand : ACommand<UpdateEducatorDTO>, IRequest<UpdateEducatorResult>
    {
        public UpdateEducatorCommand(UpdateEducatorDTO model) : base(model)
        {
        }
    }
}
