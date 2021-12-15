using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public UpdateStudentCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<UpdateStudentResult> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!Enum.GetValues(typeof(ELanguageGroup)).Cast<ELanguageGroup>().Contains(request.Model.LanguageGroup), EErrorCode.LanguageGrupeNotExitst);
            Guard.IsTrue(!Enum.GetValues(typeof(EGender)).Cast<EGender>().Contains(request.Model.Gender), EErrorCode.UndefinedGender);
            Guard.IsTrue(!_schoolDbManager.ClassExist(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);

            var studentEntry = _schoolDbManager.GetStudent(s => s.Id == request.Model.Id);

            Guard.IsTrue(studentEntry == null, EErrorCode.StudentNotExist);

            studentEntry = modifyStudent(studentEntry, request.Model);

            _schoolDbManager.UpdateStudent(studentEntry);

            await _schoolDbManager.SaveChangesAsync();

            return new UpdateStudentResult { Id = studentEntry.Id };
        }

        private StudentEntry modifyStudent(StudentEntry studentEntry, UpdateStudentDTO newStudent)
        {
            studentEntry.ClassId       = newStudent.ClassId;
            studentEntry.FirstName     = newStudent.FirstName;
            studentEntry.Surname       = newStudent.Surname;
            studentEntry.Gender        = newStudent.Gender;
            studentEntry.LanguageGroup = newStudent.LanguageGroup;

            return studentEntry;
        }

        private void clearCache(Guid studentId)
        {
            var keys = new List<string>
            {
                Constants.GetAllStudentsQueryKey,
                string.Format(Constants.GetStudentByIdQueryFormatKey, studentId.ToString())
            };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
