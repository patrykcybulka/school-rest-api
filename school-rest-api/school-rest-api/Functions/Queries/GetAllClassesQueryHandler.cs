using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, GetAllClassesResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbManager;

        public GetAllClassesQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbManager)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbManager  = redisDbManager;
        }

        public async Task<GetAllClassesResult> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            var key = nameof(GetAllClassesQuery);

            IEnumerable<ClassEntry> classEntries = null;

            classEntries = await _redisDbManager.GetDataAsync<List<ClassEntry>>(key);

            if (classEntries == null)
            {
                classEntries = _schoolDbManager.GetAllClass();
                _redisDbManager.SetDataAsync(key, classEntries);
            }

            return new GetAllClassesResult
            {
                Classes = new List<GetAllClassesItem>(classEntries.Select(c => new GetAllClassesItem
                {
                    Id   = c.Id,
                    Name = c.Name
                }))
            };
        }
    }
}
