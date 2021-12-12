using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndGroupQuery : AFunction<GetStudentsByClassAndGroupQueryDTO>, IRequest<GetStudentsByClassAndGroupQueryResult>
    {
        public GetStudentsByClassAndGroupQuery(GetStudentsByClassAndGroupQueryDTO model) : base(model)
        {
        }
    }
}
