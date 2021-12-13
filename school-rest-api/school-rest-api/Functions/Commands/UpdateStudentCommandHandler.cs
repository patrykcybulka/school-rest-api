using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public UpdateStudentCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<UpdateStudentResult> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!Enum.GetValues(typeof(ELanguageGroup)).Cast<ELanguageGroup>().Contains(request.Model.LanguageGroup), EErrorCode.LanguageGrupeNotExitst);
            Guard.IsTrue(!Enum.GetValues(typeof(EGender)).Cast<EGender>().Contains(request.Model.Gender), EErrorCode.UndefinedGender);
            Guard.IsTrue(!_schoolDbContext.Classes.Any(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);

            var studentEntry = _schoolDbContext.Students.FirstOrDefault(s => s.Id == request.Model.Id);

            Guard.IsTrue(studentEntry == null, EErrorCode.StudentNotExist);

            studentEntry = modifyStudent(studentEntry, request.Model);

            _schoolDbContext.Students.Update(studentEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

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
                nameof(GetAllStudentsQuery),
                nameof(GetStudentByIdQuery) + studentId.ToString(),
            };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
