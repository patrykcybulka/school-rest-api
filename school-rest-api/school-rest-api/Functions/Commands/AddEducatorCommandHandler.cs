using MediatR;
using school_rest_api.DbContexts;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.Results;

namespace school_rest_api.Functions.Commands
{
    public class AddEducatorCommandHandler : IRequestHandler<AddEducatorCommand, AddEducatorResult>
    {
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IRedisDbHelper  _redisDbHelper;

        public AddEducatorCommandHandler(SchoolDbContext schoolDbContext, IRedisDbHelper redisDbHelper)
        {
            _schoolDbContext = schoolDbContext;
            _redisDbHelper   = redisDbHelper;
        }

        public async Task<AddEducatorResult> Handle(AddEducatorCommand request, CancellationToken cancellationToken)
        {
            Guard.IsTrue(!_schoolDbContext.Classes.Any(c => c.Id == request.Model.ClassId), EErrorCode.ClassNotExist);

            var educatorEntry = new EducatorEntry
            {
                Id        = Guid.NewGuid(),
                ClassId   = request.Model.ClassId,
                FirstName = request.Model.FirstName,
                Surname   = request.Model.Surname
            };

            _schoolDbContext.Educators.Add(educatorEntry);
            
            await _schoolDbContext.SaveChangesAsync(cancellationToken);

            clearCache();

            return new AddEducatorResult { Id = educatorEntry.Id };
        }

        private void clearCache()
        {
            var keys = new List<string> { nameof(GetAllEducatorsQuery) };

            _redisDbHelper.RemoveOldDataAsync(keys);
        }
    }
}
