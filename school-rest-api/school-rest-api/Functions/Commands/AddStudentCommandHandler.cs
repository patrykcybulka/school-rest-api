using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, AddStudentResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public AddStudentCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddStudentResult> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!_schoolDbContext.Classes.Any(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);
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

            _schoolDbContext.Students.Add(studentEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            clearCache();

            return new AddStudentResult { Id = studentEntry.Id };
        }

        private void clearCache()
        {
            var keys = new List<string> { nameof(GetAllStudentsQuery) };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
