using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, GetAllClassesResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public GetAllClassesQueryHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetAllClassesResult> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetAllClassesQuery);

            List<ClassEntry> classEntries = null;

            classEntries = await _redisDbHelper.GetDataAsync<List<ClassEntry>>(key);

            if (classEntries == null)
            {
                classEntries = _schoolDbContext.Classes.ToList();
                _redisDbHelper.SetDataAsync(key, classEntries);
            }

            return new GetAllClassesResult
            {
                Classes = new List<GetAllClassesQueryItem>(classEntries.Select(c => new GetAllClassesQueryItem
                {
                    Id   = c.Id,
                    Name = c.Name
                }))
            };
        }
    }
}
