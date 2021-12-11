using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteEducatorCommandHandler : IRequestHandler<DeleteEducatorCommand, EmptyObjectResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public DeleteEducatorCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<EmptyObjectResult> Handle(DeleteEducatorCommand request, CancellationToken cancellationToken)
        {
            var educatorEntry = _schoolDbContext.Educators.FirstOrDefault(c => c.Id == request.Model.Id);

            Guard.IsTrue(educatorEntry == null, EErrorCode.EducatorNotExist);

            _schoolDbContext.Educators.Remove(educatorEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return EmptyObjectResult.Result;
        }
    }
}
