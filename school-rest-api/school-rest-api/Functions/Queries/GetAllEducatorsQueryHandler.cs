using MediatR;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllEducatorsQueryHandler : IRequestHandler<GetAllEducatorsQuery, GetAllEducatorsResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public GetAllEducatorsQueryHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<GetAllEducatorsResult> Handle(GetAllEducatorsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<EducatorEntry> educatorsEntries = null;

            educatorsEntries = await _redisDbHelper.GetDataAsync<List<EducatorEntry>>(Constants.GetAllEducatorsQueryKey);

            if (educatorsEntries == null)
            {
                educatorsEntries = _schoolDbManager.GetAllEducator();

                _redisDbHelper.SetDataAsync(Constants.GetAllEducatorsQueryKey, educatorsEntries);
            }

            return new GetAllEducatorsResult
            {
                Educators = new List<GetAllEducatorsItem>(educatorsEntries.Select(e => new GetAllEducatorsItem 
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
