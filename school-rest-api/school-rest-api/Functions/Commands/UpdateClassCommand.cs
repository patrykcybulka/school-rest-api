using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateClassCommand : AFunction<UpdateClassDTO>, IRequest<UpdateClassResult>
    {
        public UpdateClassCommand(UpdateClassDTO model) : base(model)
        {
        }
    }
}
