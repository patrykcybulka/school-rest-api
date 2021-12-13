using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetEducatorByIdQueryHandler : IRequestHandler<GetEducatorByIdQuery, GetEducatorByIdQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetEducatorByIdQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetEducatorByIdQueryResult> Handle(GetEducatorByIdQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetEducatorByIdQuery) + request.Model.Id.ToString();

            EducatorEntry educatorEntry = null;

            educatorEntry = await _redisDbHelper.GetDataAsync<EducatorEntry>(key);

            if (educatorEntry == null)
            {
                educatorEntry = _schoolDbContext.Educators.FirstOrDefault(e => e.Id == request.Model.Id);

                Guard.IsTrue(educatorEntry == null, EErrorCode.EducatorNotExist);

                _redisDbHelper.SetDataAsync(key, educatorEntry);
            }

            return new GetEducatorByIdQueryResult
            {
                ClassId   = educatorEntry.ClassId,
                FirstName = educatorEntry.FirstName,
                Surname   = educatorEntry.Surname
            };
        }
    }
}
