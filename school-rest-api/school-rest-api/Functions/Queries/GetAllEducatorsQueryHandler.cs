using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllEducatorsQueryHandler : IRequestHandler<GetAllEducatorsQuery, GetAllEducatorsQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetAllEducatorsQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetAllEducatorsQueryResult> Handle(GetAllEducatorsQuery request, CancellationToken cancellationToken)
        {
            var educatorsEntries = _schoolDbContext.Students.ToList();

            return Task.FromResult(new GetAllEducatorsQueryResult
            {
                Educators = new List<GetAllEducatorsQueryItem>(educatorsEntries.Select(e => new GetAllEducatorsQueryItem 
                { 
                    Id        = e.Id,
                    ClassId   = e.ClassId,
                    FirstName = e.FirstName,
                    Surname   = e.Surname 
                }))
            });
        }
    }
}
