using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, GetClassByIdQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetClassByIdQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetClassByIdQueryResult> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var classEntry = _schoolDbContext.Classes.FirstOrDefault(c => c.Id == request.Model.Id);

            Guard.IsTrue(classEntry == null, EErrorCode.ClassNotExist);

            return Task.FromResult(new GetClassByIdQueryResult { Name = classEntry.Name });
        }
    }
}
