using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, GetClassByIdResult>
    {
        private readonly ISchoolDbManager _schoolDbContext;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetClassByIdQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbContext = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetClassByIdResult> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            ClassEntry classEntry = null;

            var key = string.Format(Constants.GetClassByIdQueryFormatKey, request.Model.Id);

            classEntry = await _redisDbHelper.GetDataAsync<ClassEntry>(key);

            if (classEntry == null)
            {
                classEntry = _schoolDbContext.GetClass(c => c.Id == request.Model.Id);

                Guard.IsTrue(classEntry == null, EErrorCode.ClassNotExist);

                _redisDbHelper.SetDataAsync(key, classEntry);
            }

            return new GetClassByIdResult
            {
                Name = classEntry.Name
            };
        }
    }
}
