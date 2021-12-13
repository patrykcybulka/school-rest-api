using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, EmptyObjectResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public DeleteStudentCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<EmptyObjectResult> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentEntry = _schoolDbContext.Students.FirstOrDefault(c => c.Id == request.Model.Id);

            Guard.IsTrue(studentEntry == null, EErrorCode.StudentNotExist);

            _schoolDbContext.Students.Remove(studentEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return EmptyObjectResult.Result;
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
