using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, UpdateClassResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public UpdateClassCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<UpdateClassResult> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var className = char.ToUpper(request.Model.Name);

            Guard.IsTrue(!Constants.RangeOfClassNames.Contains(className), EErrorCode.NotSupportedClassName);

            var classEntries = _schoolDbManager.GetClasses(c => c.Id == request.Model.Id || c.Name == className);

            Guard.IsTrue(classEntries == null || !classEntries.Any(c => c.Id == request.Model.Id), EErrorCode.ClassNotExist);
            Guard.IsTrue(classEntries.Count() > 1, EErrorCode.NameIsUsed);

            var classEntry = modifyClass(classEntries.First(), className);

            _schoolDbManager.UpdateClass(classEntry);

            await _schoolDbManager.SaveChangesAsync();

            clearCache(request.Model.Id);

            return new UpdateClassResult { Id = classEntry.Id };
        }

        private ClassEntry modifyClass(ClassEntry classEntry, char className)
        {
            classEntry.Name = className;

            return classEntry;
        }

        private void clearCache(Guid classId)
        {
            var keys = new List<string>
            {
                nameof(GetAllClassesQuery),
                nameof(GetClassByIdQuery) + classId.ToString()
            };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
