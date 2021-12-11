using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, GetAllStudentsQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetAllStudentsQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetAllStudentsQueryResult> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var studentsEntries = _schoolDbContext.Students.ToList();

            return Task.FromResult(new GetAllStudentsQueryResult
            {
                Students = new List<GetAllStudentsQueryItem>(studentsEntries.Select(s => new GetAllStudentsQueryItem
                {
                    Id            = s.Id,
                    ClassId       = s.ClassId,
                    FirstName     = s.FirstName,
                    Surname       = s.Surname,
                    Gender        = s.Gender,
                    LanguageGroup = s.LanguageGroup
                }))
            });
        }
    }
}
