using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllEducatorsQueryHandler : IRequestHandler<GetAllEducatorsQuery, GetAllEducatorsQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetAllEducatorsQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetAllEducatorsQueryResult> Handle(GetAllEducatorsQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetAllEducatorsQuery);

            List<EducatorEntry> educatorsEntries = null;

            educatorsEntries = await _redisDbHelper.GetDataAsync<List<EducatorEntry>>(key);

            if (educatorsEntries == null)
            {
                educatorsEntries = _schoolDbContext.Educators.ToList();
                _redisDbHelper.SetDataAsync(key, educatorsEntries);
            }

            return new GetAllEducatorsQueryResult
            {
                Educators = new List<GetAllEducatorsQueryItem>(educatorsEntries.Select(e => new GetAllEducatorsQueryItem 
                { 
                    Id        = e.Id,
                    ClassId   = e.ClassId,
                    FirstName = e.FirstName,
                    Surname   = e.Surname 
                }))
            };
        }
    }
}
