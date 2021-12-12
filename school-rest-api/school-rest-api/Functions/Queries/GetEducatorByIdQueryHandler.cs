using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetEducatorByIdQueryHandler : IRequestHandler<GetEducatorByIdQuery, GetEducatorByIdQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetEducatorByIdQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetEducatorByIdQueryResult> Handle(GetEducatorByIdQuery request, CancellationToken cancellationToken)
        {
            var educatorEntry = _schoolDbContext.Educators.FirstOrDefault(e => e.Id == request.Model.Id);

            Guard.IsTrue(educatorEntry == null, EErrorCode.EducatorNotExist);

            return Task.FromResult(new GetEducatorByIdQueryResult
            {
                ClassId   = educatorEntry.ClassId,
                FirstName = educatorEntry.FirstName,
                Surname   = educatorEntry.Surname
            });
        }
    }
}
