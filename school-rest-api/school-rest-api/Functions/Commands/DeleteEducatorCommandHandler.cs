using MediatR;
using school_rest_api.Databases;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteEducatorCommandHandler : IRequestHandler<DeleteEducatorCommand, EmptyObjectResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public DeleteEducatorCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<EmptyObjectResult> Handle(DeleteEducatorCommand request, CancellationToken cancellationToken)
        {
            var educatorEntry = _schoolDbManager.GetEducator(c => c.Id == request.Model.Id);

            Guard.IsTrue(educatorEntry == null, EErrorCode.EducatorNotExist);

            _schoolDbManager.RemoveEducator(educatorEntry);

            await _schoolDbManager.SaveChangesAsync();

            clearCache(request.Model.Id);

            return EmptyObjectResult.Result;
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
