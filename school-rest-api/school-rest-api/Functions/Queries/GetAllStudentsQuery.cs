using MediatR;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetAllStudentsQuery : IRequest<GetAllStudentsResult>
    {
    }
}
