using school_rest_api.Entries;
using System.Linq.Expressions;

namespace school_rest_api.Databases
{
    public class SchoolDbManager : ISchoolDbManager
    {
        private readonly SchoolDbContext _context;

        public SchoolDbManager(SchoolDbContext context)
        {
            _context = context;
        }

        public void AddClass(ClassEntry classEntry)
        {
            _context.Classes.Add(classEntry);
        }

        public void AddEducator(EducatorEntry educatorEntry)
        {
            _context.Educators.Add(educatorEntry);
        }

        public void AddStudent(StudentEntry studentEntry)
        {
            _context.Students.Add(studentEntry);
        }

        public bool ClassExist(Expression<Func<ClassEntry, bool>> expression)
        {
            return _context.Classes.Any(expression);
        }

        public IEnumerable<ClassEntry> GetAllClass()
        {
            return _context.Classes.ToList();
        }

        public IEnumerable<EducatorEntry> GetAllEducator()
        {
            return _context.Educators.ToList();
        }

        public IEnumerable<StudentEntry> GetAllStudent()
        {
            return _context.Students.ToList();
        }

        public ClassEntry GetClass(Expression<Func<ClassEntry, bool>> expression)
        {
            return _context.Classes.FirstOrDefault(expression);
        }

        public IEnumerable<ClassEntry> GetClasses(Expression<Func<ClassEntry, bool>> expression)
        {
            return _context.Classes.Where(expression).ToList();
        }

        public EducatorEntry GetEducator(Expression<Func<EducatorEntry, bool>> expression)
        {
            return _context.Educators.FirstOrDefault(expression);
        }

        public IEnumerable<EducatorEntry> GetEducators(Expression<Func<EducatorEntry, bool>> expression)
        {
            return _context.Educators.Where(expression).ToList();
        }

        public StudentEntry GetStudent(Expression<Func<StudentEntry, bool>> expression)
        {
            return _context.Students.FirstOrDefault(expression);
        }

        public IEnumerable<StudentEntry> GetStudents(Expression<Func<StudentEntry, bool>> expression)
        {
            return _context.Students.Where(expression).ToList();
        }

        public void RemoveClass(ClassEntry classEntry)
        {
            _context.Classes.Remove(classEntry);
        }

        public void RemoveEducator(EducatorEntry educatorEntry)
        {
            _context.Educators.Remove(educatorEntry);
        }

        public void RenoveStudent(StudentEntry studentEntry)
        {
            _context.Students.Remove(studentEntry);
        }

        public Task<int> SaveChangesAsync()
        {
           return _context.SaveChangesAsync();
        }

        public void UpdateClass(ClassEntry classEntry)
        {
            _context.Classes.Update(classEntry);
        }

        public void UpdateEducator(EducatorEntry educatorEntry)
        {
            _context.Educators.Update(educatorEntry);
        }

        public void UpdateManyStudent(IEnumerable<StudentEntry> studentEntry)
        {
            _context.Students.UpdateRange(studentEntry);
        }

        public void UpdateStudent(StudentEntry studentEntry)
        {
            _context.Students.Update(studentEntry);
        }
    }
}
