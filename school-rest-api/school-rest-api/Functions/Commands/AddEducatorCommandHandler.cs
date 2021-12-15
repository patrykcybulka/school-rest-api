using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddEducatorCommandHandler : IRequestHandler<AddEducatorCommand, AddEducatorResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public AddEducatorCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddEducatorResult> Handle(AddEducatorCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!_schoolDbManager.ClassExist(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);

            var educatorEntry = new EducatorEntry
            {
                Id        = Guid.NewGuid(),
                ClassId   = request.Model.ClassId,
                FirstName = request.Model.FirstName,
                Surname   = request.Model.Surname
            };

            _schoolDbManager.AddEducator(educatorEntry);
            
            await _schoolDbManager.SaveChangesAsync();

            clearCache();

            return new AddEducatorResult { Id = educatorEntry.Id };
        }

        private void clearCache()
        {
            var keys = new List<string> { Constants.GetAllEducatorsQueryKey };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
