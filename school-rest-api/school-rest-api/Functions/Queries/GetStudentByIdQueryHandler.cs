using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, GetStudentByIdQueryResult>
    {
        private readonly SchoolDbContext _schoolDbContext;

        public GetStudentByIdQueryHandler(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public Task<GetStudentByIdQueryResult> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var studentsEntry = _schoolDbContext.Students.FirstOrDefault(s => s.Id == request.Model.Id);

            Guard.IsTrue(studentsEntry == null, EErrorCode.StudentNotExist);

            return Task.FromResult(new GetStudentByIdQueryResult
            {
                ClassId       = studentsEntry.ClassId,
                FirstName     = studentsEntry.FirstName,
                Surname       = studentsEntry.Surname,
                Gender        = studentsEntry.Gender,
                LanguageGroup = studentsEntry.LanguageGroup
            });
        }
    }
}
