using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, EmptyObjectResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public DeleteClassCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<EmptyObjectResult> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var classEntry = _schoolDbContext.Classes.FirstOrDefault(c => c.Id == request.Model.Id);

            Guard.IsTrue(classEntry == null, EErrorCode.ClassNotExist);

            _schoolDbContext.Classes.Remove(classEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return EmptyObjectResult.Result;
        }
    }
}
