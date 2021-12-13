using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class UpdateEducatorCommandHandler : IRequestHandler<UpdateEducatorCommand, UpdateEducatorResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public UpdateEducatorCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<UpdateEducatorResult> Handle(UpdateEducatorCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!_schoolDbManager.ClassExist(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);

            var educatorEntries = _schoolDbManager.GetEducators( e => e.Id == request.Model.Id || e.ClassId == request.Model.ClassId);

            Guard.IsTrue(educatorEntries == null, EErrorCode.EducatorNotExist);
            Guard.IsTrue(educatorEntries.Count() > 1 || educatorEntries.Any( e => e.ClassId == request.Model.ClassId), EErrorCode.ClassAssignedToAnotherEducator);

            var educatorEntry = modifyEducator(educatorEntries.First(), request.Model);

            _schoolDbManager.UpdateEducator(educatorEntry);

            await _schoolDbManager.SaveChangesAsync();

            clearCache(request.Model.ClassId);

            return new UpdateEducatorResult { Id = educatorEntry.Id };
        }

        private EducatorEntry modifyEducator(EducatorEntry edycatorEntry, UpdateEducatorDTO newEducator )
        {
            edycatorEntry.ClassId   = newEducator.ClassId;
            edycatorEntry.FirstName = newEducator.FirstName;
            edycatorEntry.Surname   = newEducator.Surname;

            return edycatorEntry;
        }

        private void clearCache(Guid educatorId)
        {
            var keys = new List<string>
            {
                nameof(GetAllEducatorsQuery),
                nameof(GetEducatorByIdQuery) + educatorId.ToString(),
            };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
