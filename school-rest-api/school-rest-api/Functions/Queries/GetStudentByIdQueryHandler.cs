using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, GetStudentByIdResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetStudentByIdQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetStudentByIdResult> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetStudentByIdQuery) + request.Model.Id.ToString();

            StudentEntry studentsEntry = null;

            studentsEntry = await _redisDbHelper.GetDataAsync<StudentEntry>(key);

            if (studentsEntry == null)
            {
                studentsEntry = _schoolDbManager.GetStudent(s => s.Id == request.Model.Id);

                Guard.IsTrue(studentsEntry == null, EErrorCode.StudentNotExist);

                _redisDbHelper.SetDataAsync(key, studentsEntry);
            }

            return new GetStudentByIdResult
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
