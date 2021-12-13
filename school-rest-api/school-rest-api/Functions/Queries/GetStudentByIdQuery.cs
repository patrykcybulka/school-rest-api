using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentByIdQuery : AFunction<GetStudentByIdDTO>, IRequest<GetStudentByIdResult>
    {
        public GetStudentByIdQuery(GetStudentByIdDTO model) : base(model)
        {
        }
    }
}
