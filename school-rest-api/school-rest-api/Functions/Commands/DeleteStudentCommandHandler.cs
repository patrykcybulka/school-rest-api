using MediatR;
using school_rest_api.Databases;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, EmptyObjectResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public DeleteStudentCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<EmptyObjectResult> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentEntry = _schoolDbManager.GetStudent(s => s.Id == request.Model.Id);

            Guard.IsTrue(studentEntry == null, EErrorCode.StudentNotExist);

            _schoolDbManager.RenoveStudent(studentEntry);

            await _schoolDbManager.SaveChangesAsync();

            return EmptyObjectResult.Result;
        }

        private void clearCache(Guid studentId)
        {
            var keys = new List<string>
            {
                Constants.GetAllStudentsQueryKey,
                string.Format(GetStudentByIdQuery, studentId.ToString())
            };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
