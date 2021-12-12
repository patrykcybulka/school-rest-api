using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndGroupQueryHandler : IRequestHandler<GetStudentsByClassAndGroupQuery, GetStudentsByClassAndGroupQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetStudentsByClassAndGroupQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetStudentsByClassAndGroupQueryResult> Handle(GetStudentsByClassAndGroupQuery request, CancellationToken cancellationToken)
        {
            var studentsEntries = _schoolDbContext.Students.Where(s => s.ClassId == request.Model.Id && s.LanguageGroup == request.Model.LanguageGroup);

            return Task.FromResult(new GetStudentsByClassAndGroupQueryResult
            {
                Students = new List<GetStudentsByClassAndGroupQueryItem>(studentsEntries.Select(s => new GetStudentsByClassAndGroupQueryItem
                {
                    Id        = s.Id,
                    FirstName = s.FirstName,
                    Surname   = s.Surname,
                    Gender    = s.Gender,
                }))
            });
        }
    }
}