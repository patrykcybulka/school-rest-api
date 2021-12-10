using MediatR;
using school_rest_api.Models;

namespace school_rest_api.Functions.Commands
{
    public class AddEducatorCommand : IRequest<Guid>
    {
        public AddEducatorDTO Model { get; }

        public AddEducatorCommand(AddEducatorDTO model)
        {
            Model = model;
        }
    }
}
