using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, AddStudentResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public AddStudentCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddStudentResult> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!_schoolDbManager.ClassExist(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);
            Guard.IsTrue(!Enum.GetValues(typeof(ELanguageGroup)).Cast<ELanguageGroup>().Contains(request.Model.LanguageGroup), EErrorCode.LanguageGrupeNotExitst);
            Guard.IsTrue(!Enum.GetValues(typeof(EGender)).Cast<EGender>().Contains(request.Model.Gender), EErrorCode.UndefinedGender);

            var studentEntry = new StudentEntry
            {
                Id            = Guid.NewGuid(),
                ClassId       = request.Model.ClassId,
                FirstName     = request.Model.FirstName,
                Surname       = request.Model.Surname,
                Gender        = request.Model.Gender,
                LanguageGroup = request.Model.LanguageGroup
            };

            _schoolDbManager.AddStudent(studentEntry);

            await _schoolDbManager.SaveChangesAsync();

            clearCache();

            return new AddStudentResult { Id = studentEntry.Id };
        }

        private void clearCache()
        {
            var keys = new List<string> { Constants.GetAllStudentsQueryKey };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
