using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, UpdateClassResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public UpdateClassCommandHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<UpdateClassResult> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var className = char.ToUpper(request.Model.Name);

            Guard.IsTrue(!Constants.RangeOfClassNames.Contains(className), EErrorCode.NotSupportedClassName);

            var classEntries = _schoolDbContext.Classes.Where(c => c.Id == request.Model.Id || c.Name == className);

            Guard.IsTrue(classEntries == null || !classEntries.Any(c => c.Id == request.Model.Id), EErrorCode.ClassNotExist);
            Guard.IsTrue(classEntries.Count() > 1, EErrorCode.NameIsUsed);

            var classEntry = modifyClass(classEntries.First(), className);

            _schoolDbContext.Classes.Update(classEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateClassResult { Id = classEntry.Id };
        }

        private ClassEntry modifyClass(ClassEntry classEntry, char className)
        {
            classEntry.Name = className;

            return classEntry;
        }
    }
}
