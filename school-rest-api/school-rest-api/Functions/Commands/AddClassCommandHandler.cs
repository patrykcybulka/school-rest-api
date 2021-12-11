using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddClassCommandHandler : IRequestHandler<AddClassCommand, AddClassResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public AddClassCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<AddClassResult> Handle(AddClassCommand request, CancellationToken cancellationToken)
        {
            var className = char.ToUpper(request.Model.Name);

            if (!Constants.RangeOfClassNames.Contains(className))
                throw new SchoolException(EErrorCode.NotSupportedClassName);

            if (_schoolDbContext.Classes.Any(c => c.Name == className))
                throw new SchoolException(EErrorCode.ClassAlreadyExists);

            var classEntry = new ClassEntry
            {
                Id   = Guid.NewGuid(),
                Name = className
            };

            _schoolDbContext.Classes.Add(classEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return new AddClassResult { Id = classEntry.Id };
        }
    }
}
