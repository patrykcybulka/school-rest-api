using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndSortedByGenderQueryHandler : IRequestHandler<GetStudentsByClassAndSortedByGenderQuery, GetStudentsSortedByGenderQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetStudentsByClassAndSortedByGenderQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetStudentsSortedByGenderQueryResult> Handle(GetStudentsByClassAndSortedByGenderQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetStudentsByClassAndSortedByGenderQuery) + request.Model.Id.ToString();

            List<StudentEntry> studentsEntries = null;

            studentsEntries = await _redisDbHelper.GetDataAsync<List<StudentEntry>>(key);

            if (studentsEntries == null)
            {
                studentsEntries = _schoolDbContext.Students.Where(s => s.ClassId == request.Model.Id).OrderByDescending(s => s.Gender == request.Model.Gender).ToList();
                _redisDbHelper.SetDataAsync(key, studentsEntries);
            }

            return new GetStudentsSortedByGenderQueryResult
            {
                Students = new List<GetStudentsSortedByGenderQueryItem>(studentsEntries.Select(s => new GetStudentsSortedByGenderQueryItem
                {
                    Id            = s.Id,
                    FirstName     = s.FirstName,
                    Surname       = s.Surname,
                    Gender        = s.Gender,
                    LanguageGroup = s.LanguageGroup
                }))
            };
        }
    }
}
