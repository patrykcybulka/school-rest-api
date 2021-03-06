using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetClassByIdQuery : AFunction<GetClassByIdDTO>, IRequest<GetClassByIdResult>
    {
        public GetClassByIdQuery(GetClassByIdDTO model) : base(model)
        {
        }
    }
}
