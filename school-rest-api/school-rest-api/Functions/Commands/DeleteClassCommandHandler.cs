using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, EmptyObjectResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public DeleteClassCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<EmptyObjectResult> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var classEntry = _schoolDbContext.Classes.FirstOrDefault(c => c.Id == request.Model.Id);

            Guard.IsTrue(classEntry == null, EErrorCode.ClassNotExist);

            _schoolDbContext.Classes.Remove(classEntry);

            var educatorId = updateEducator(request.Model.Id);
            var studentIds = updateStudents(request.Model.Id);

            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            clearCache(request.Model.Id, educatorId, studentIds);

            return EmptyObjectResult.Result;
        }

        private Guid updateEducator(Guid classId)
        {
            var educatorEntry = _schoolDbContext.Educators.FirstOrDefault(e => e.ClassId == classId);

            if (educatorEntry == null)
            {
                return Guid.Empty;
            }

            educatorEntry.ClassId = Guid.Empty;

            _schoolDbContext.Educators.Update(educatorEntry);

            return educatorEntry.Id;
        }

        private IEnumerable<Guid> updateStudents(Guid classId)
        {
            var studentIds = _schoolDbContext.Students.Where(s => s.ClassId == classId).ToList();

            return studentIds.Select(s => s.Id);
        }

        private void clearCache(Guid classId, Guid educatorId, IEnumerable<Guid> studentIds)
        {
            var keys = new List<string>
            { 
                nameof(GetAllClassesQuery),
                nameof(GetAllEducatorsQuery),
                nameof(GetAllStudentsQuery),
                nameof(GetClassByIdQuery) + classId.ToString()
            };

            if (educatorId != Guid.Empty)
            {
                keys.Add(nameof(GetEducatorByIdQuery) + educatorId.ToString());
            }

            if (studentIds.Count() != 0)
            {
                foreach (var userId in studentIds)
                {
                    keys.Add(nameof(GetStudentByIdQuery) + userId.ToString());
                }
            }

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
