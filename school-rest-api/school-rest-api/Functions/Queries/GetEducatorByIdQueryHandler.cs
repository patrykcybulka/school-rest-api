using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetEducatorByIdQueryHandler : IRequestHandler<GetEducatorByIdQuery, GetEducatorByIdResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetEducatorByIdQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetEducatorByIdResult> Handle(GetEducatorByIdQuery request, CancellationToken cancellationToken)
        {
            EducatorEntry educatorEntry = null;

            var key = string.Format(Constants.GetEducatorByIdQueryFormatKey, request.Model.Id);

            educatorEntry = await _redisDbHelper.GetDataAsync<EducatorEntry>(key);

            if (educatorEntry == null)
            {
                educatorEntry = _schoolDbManager.GetEducator(e => e.Id == request.Model.Id);

                Guard.IsTrue(educatorEntry == null, EErrorCode.EducatorNotExist);

                _redisDbHelper.SetDataAsync(key, educatorEntry);
            }

            return new GetEducatorByIdResult
            {
                ClassId   = educatorEntry.ClassId,
                FirstName = educatorEntry.FirstName,
                Surname   = educatorEntry.Surname
            };
        }
    }
}
