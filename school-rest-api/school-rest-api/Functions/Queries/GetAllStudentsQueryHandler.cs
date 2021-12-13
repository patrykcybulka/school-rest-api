using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, GetAllStudentsQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper _redisDbHelper;

        public GetAllStudentsQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetAllStudentsQueryResult> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetAllStudentsQuery);

            List<StudentEntry> studentsEntries = null;

            studentsEntries = await _redisDbHelper.GetDataAsync<List<StudentEntry>>(key);

            if (studentsEntries == null)
            {
                studentsEntries = _schoolDbContext.Students.ToList();
                _redisDbHelper.SetDataAsync(key, studentsEntries);
            }

            return new GetAllStudentsQueryResult
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
            };
        }
    }
}
