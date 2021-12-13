using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetEducatorByIdQuery : AFunction<GetEducatorByIdDTO>, IRequest<GetEducatorByIdResult>
    {
        public GetEducatorByIdQuery(GetEducatorByIdDTO model) : base(model)
        {
        }
    }
}
