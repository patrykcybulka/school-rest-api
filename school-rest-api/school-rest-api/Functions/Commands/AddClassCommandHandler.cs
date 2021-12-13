using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddClassCommandHandler : IRequestHandler<AddClassCommand, AddClassResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public AddClassCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddClassResult> Handle(AddClassCommand request, CancellationToken cancellationToken)
        {
            var className = char.ToUpper(request.Model.Name);

            Guard.IsTrue(!Constants.RangeOfClassNames.Contains(className), EErrorCode.NotSupportedClassName);
            Guard.IsTrue(_schoolDbContext.Classes.Any(c => c.Name == className), EErrorCode.ClassAlreadyExists);

            var classEntry = new ClassEntry
            {
                Id   = Guid.NewGuid(),
                Name = className
            };

            _schoolDbContext.Classes.Add(classEntry);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            clearCache();

            return new AddClassResult { Id = classEntry.Id };
        }

        private void clearCache()
        {
            var keys = new List<string> { nameof(GetAllClassesQuery) };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
