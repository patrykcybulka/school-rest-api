using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, GetStudentByIdQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetStudentByIdQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetStudentByIdQueryResult> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetStudentByIdQuery) + request.Model.Id.ToString();

            StudentEntry studentsEntry = null;

            studentsEntry = await _redisDbHelper.GetDataAsync<StudentEntry>(key);

            if (studentsEntry == null)
            {
                studentsEntry = _schoolDbContext.Students.FirstOrDefault(s => s.Id == request.Model.Id);

                Guard.IsTrue(studentsEntry == null, EErrorCode.StudentNotExist);

                _redisDbHelper.SetDataAsync(key, studentsEntry);
            }

            return new GetStudentByIdQueryResult
            {
                ClassId       = studentsEntry.ClassId,
                FirstName     = studentsEntry.FirstName,
                Surname       = studentsEntry.Surname,
                Gender        = studentsEntry.Gender,
                LanguageGroup = studentsEntry.LanguageGroup
            };
        }
    }
}
