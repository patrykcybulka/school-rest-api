using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentsByClassAndSortedByGenderQueryHandler : IRequestHandler<GetStudentsByClassAndSortedByGenderQuery, GetStudentsSortedByGenderResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetStudentsByClassAndSortedByGenderQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetStudentsSortedByGenderResult> Handle(GetStudentsByClassAndSortedByGenderQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetStudentsByClassAndSortedByGenderQuery) + request.Model.Id.ToString();

            IEnumerable<StudentEntry> studentsEntries = null;

            studentsEntries = await _redisDbHelper.GetDataAsync<List<StudentEntry>>(key);

            if (studentsEntries == null)
            {
                studentsEntries = _schoolDbManager.GetStudents(s => s.ClassId == request.Model.Id).OrderByDescending(s => s.Gender == request.Model.Gender);
                _redisDbHelper.SetDataAsync(key, studentsEntries);
            }

            return new GetStudentsSortedByGenderResult
            {
                Students = new List<GetStudentsSortedByGenderItem>(studentsEntries.Select(s => new GetStudentsSortedByGenderItem
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
