using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, GetClassByIdResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetClassByIdQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetClassByIdResult> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetClassByIdQuery) + request.Model.Id.ToString();

            ClassEntry classEntry = null;

            classEntry = await _redisDbHelper.GetDataAsync<ClassEntry>(key);

            if (classEntry == null)
            {
                classEntry = _schoolDbContext.Classes.FirstOrDefault(c => c.Id == request.Model.Id);

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
