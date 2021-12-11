using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Models.Results;
using school_rest_api.Models.Results.Items;

namespace school_rest_api.Functions.Queries
{
    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, GetAllClassesResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetAllClassesQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetAllClassesResult> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            var classEntries = _schoolDbContext.Classes.ToList();

            return Task.FromResult(new GetAllClassesResult
            {
                Classes = new List<GetAllClassesQueryItem>(classEntries.Select(c => new GetAllClassesQueryItem
                {
                    Id   = c.Id,
                    Name = c.Name
                }))
            });
        }
    }
}
