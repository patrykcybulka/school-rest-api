using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, GetAllStudentsResult>
    {
        private readonly ISchoolDbManager _schoolDbContext;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetAllStudentsQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbContext = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetAllStudentsResult> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<StudentEntry> studentsEntries = null;

            studentsEntries = await _redisDbHelper.GetDataAsync<List<StudentEntry>>(Constants.GetAllStudentsQueryKey);

            if (studentsEntries == null)
            {
                studentsEntries = _schoolDbContext.GetAllStudent();

                _redisDbHelper.SetDataAsync(Constants.GetAllStudentsQueryKey, studentsEntries);
            }

            return new GetAllStudentsResult
            {
                Students = new List<GetAllStudentsItem>(studentsEntries.Select(s => new GetAllStudentsItem
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
