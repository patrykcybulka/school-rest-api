using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndSortedByGenderQueryHandler : IRequestHandler<GetStudentsByClassAndSortedByGenderQuery, GetStudentsSortedByGenderQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetStudentsByClassAndSortedByGenderQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetStudentsSortedByGenderQueryResult> Handle(GetStudentsByClassAndSortedByGenderQuery request, CancellationToken cancellationToken)
        {
            var studentsEntries = _schoolDbContext.Students.Where(s => s.ClassId == request.Model.Id).OrderBy( s => s.Gender == request.Model.Gender);

            return Task.FromResult(new GetStudentsSortedByGenderQueryResult
            {
                Students = new List<GetStudentsSortedByGenderQueryItem>(studentsEntries.Select(s => new GetStudentsSortedByGenderQueryItem
                {
                    Id            = s.Id,
                    FirstName     = s.FirstName,
                    Surname       = s.Surname,
                    Gender        = s.Gender,
                    LanguageGroup = s.LanguageGroup
                }))
            });
        }
    }
}
