using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, EmptyObjectResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public DeleteStudentCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<EmptyObjectResult> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentEntry = _schoolDbContext.Students.FirstOrDefault(c => c.Id == request.Model.Id);

            if (studentEntry == null)
                throw new SchoolException(EErrorCode.StudentNotExist);

            _schoolDbContext.Students.Remove(studentEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return EmptyObjectResult.Result;
        }
    }
}
