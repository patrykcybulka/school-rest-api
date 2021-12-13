using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddClassCommandHandler : IRequestHandler<AddClassCommand, AddClassResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public AddClassCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddClassResult> Handle(AddClassCommand request, CancellationToken cancellationToken)
        {
            var className = char.ToUpper(request.Model.Name);

            Guard.IsTrue(!Constants.RangeOfClassNames.Contains(className), EErrorCode.NotSupportedClassName);
            Guard.IsTrue(_schoolDbManager.ClassExist(c => c.Name == className), EErrorCode.ClassAlreadyExists);

            var classEntry = new ClassEntry
            {
                Id   = Guid.NewGuid(),
                Name = className
            };

            _schoolDbManager.AddClass(classEntry);

            await _schoolDbManager.SaveChangesAsync();

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
