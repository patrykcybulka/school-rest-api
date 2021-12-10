using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;

namespace school_rest_api.Functions.Commands
{
    public class AddEducatorCommandHandler : IRequestHandler<AddEducatorCommand, Guid>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public AddEducatorCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<Guid> Handle(AddEducatorCommand request, CancellationToken cancellationToken)
        {
            if (!_schoolDbContext.Classes.Any(c => c.Id == request.Model.ClassId))
                throw new SchoolException(EErrorCode.ClassNotExist);

            var educatorEntry = new EducatorEntry
            {
                Id        = Guid.NewGuid(),
                ClassId   = request.Model.ClassId,
                FirstName = request.Model.FirstName,
                Surname   = request.Model.Surname
            };

            _schoolDbContext.Educators.Add(educatorEntry);
            
            await _schoolDbContext.SaveChangesAsync();

            return educatorEntry.Id;
        }
    }
}
