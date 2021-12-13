using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndGroupQueryHandler : IRequestHandler<GetStudentsByClassAndGroupQuery, GetStudentsByClassAndGroupResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetStudentsByClassAndGroupQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetStudentsByClassAndGroupResult> Handle(GetStudentsByClassAndGroupQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetStudentsByClassAndGroupQuery) + request.Model.Id.ToString();

            List<StudentEntry> studentsEntries = null;

            studentsEntries = await _redisDbHelper.GetDataAsync<List<StudentEntry>>(key);

            if (studentsEntries == null)
            {
                studentsEntries = _schoolDbContext.Students.Where(s => s.ClassId == request.Model.Id && s.LanguageGroup == request.Model.LanguageGroup).ToList();
                _redisDbHelper.SetDataAsync(key, studentsEntries);
            }

            return new GetStudentsByClassAndGroupResult
            {
                Students = new List<GetStudentsByClassAndGroupItem>(studentsEntries.Select(s => new GetStudentsByClassAndGroupItem
                {
                    Id        = s.Id,
                    FirstName = s.FirstName,
                    Surname   = s.Surname,
                    Gender    = s.Gender,
                }))
            };
        }
    }
}