using MediatR;
using school_rest_api.Databases;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, EmptyObjectResult>
    {
        private readonly ISchoolDbManager _schoolDbManager;
        private readonly IRedisDbManager  _redisDbHelper;

        public DeleteClassCommandHandler(ISchoolDbManager schoolDbManager, IRedisDbManager redisDbHelper)
        {
            _schoolDbManager = schoolDbManager;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<EmptyObjectResult> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var classEntry = _schoolDbManager.GetClass(c => c.Id == request.Model.Id);

            Guard.IsTrue(classEntry == null, EErrorCode.ClassNotExist);

            _schoolDbManager.RemoveClass(classEntry);

            var educatorId = updateEducator(request.Model.Id);
            var studentIds = updateStudents(request.Model.Id);

            await _schoolDbManager.SaveChangesAsync();

            clearCache(request.Model.Id, educatorId, studentIds);

            return EmptyObjectResult.Result;
        }

        private Guid updateEducator(Guid classId)
        {
            var educatorEntry = _schoolDbManager.GetEducator(e => e.ClassId == classId);

            if (educatorEntry == null)
            {
                return Guid.Empty;
            }

            educatorEntry.ClassId = Guid.Empty;

            _schoolDbManager.UpdateEducator(educatorEntry);

            return educatorEntry.Id;
        }

        private IEnumerable<Guid> updateStudents(Guid classId)
        {
            var studentsEntries = _schoolDbManager.GetStudents(s => s.ClassId == classId).ToList();

            foreach (var studentEntry in studentsEntries)
            {
                studentEntry.ClassId = Guid.Empty;
            }

            _schoolDbManager.UpdateManyStudent(studentsEntries);

            return studentsEntries.Select(s => s.Id);
        }

        private void clearCache(Guid classId, Guid educatorId, IEnumerable<Guid> studentIds)
        {
            var keys = new List<string>
            {
                Constants.GetAllClassesQueryKey,
                Constants.GetAllEducatorsQueryKey,
                Constants.GetAllStudentsQueryKey,
                string.Format(Constants.GetClassByIdQueryFormatKey, classId.ToString())
            };

            if (educatorId != Guid.Empty)
            {
                keys.Add(string.Format(Constants.GetEducatorByIdQueryFormatKey, educatorId.ToString()));
            }

            if (studentIds.Count() != 0)
            {
                foreach (var userId in studentIds)
                {
                    keys.Add(string.Format(Constants.GetStudentByIdQueryFormatKey, userId.ToString()));
                }
            }

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
