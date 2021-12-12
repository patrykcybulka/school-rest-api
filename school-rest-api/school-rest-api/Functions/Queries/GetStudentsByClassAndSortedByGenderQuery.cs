using MediatR;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndSortedByGenderQuery : AFunction<GetStudentsByClassAndSortedByGenderQueryDTO>, IRequest<GetStudentsSortedByGenderQueryResult>
    {
        public GetStudentsByClassAndSortedByGenderQuery(GetStudentsByClassAndSortedByGenderQueryDTO model) : base(model)
        {
        }
    }
}
