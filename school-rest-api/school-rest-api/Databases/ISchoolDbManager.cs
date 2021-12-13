using school_rest_api.Entries;
using System.Linq.Expressions;

namespace school_rest_api.Databases
{
    public interface ISchoolDbManager
    {
        bool ClassExist(Expression<Func<ClassEntry, bool>> expression);
        void AddClass(ClassEntry classEntry);
        void UpdateClass(ClassEntry classEntry);
        void RemoveClass(ClassEntry classEntry);
        ClassEntry GetClass(Expression<Func<ClassEntry, bool>> expression);
        IEnumerable<ClassEntry> GetClasses(Expression<Func<ClassEntry, bool>> expression);
        IEnumerable<ClassEntry> GetAllClass();

        void AddEducator(EducatorEntry educatorEntry);
        void RemoveEducator(EducatorEntry educatorEntry);
        void UpdateEducator(EducatorEntry educatorEntry);
        EducatorEntry GetEducator(Expression<Func<EducatorEntry, bool>> expression);
        IEnumerable<EducatorEntry> GetEducators(Expression<Func<EducatorEntry, bool>> expression);
        IEnumerable<EducatorEntry> GetAllEducator();

        void AddStudent(StudentEntry studentEntry);
        void RenoveStudent(StudentEntry studentEntry);
        void UpdateStudent(StudentEntry studentEntry);
        void UpdateManyStudent(IEnumerable<StudentEntry> studentEntry);
        StudentEntry GetStudent(Expression<Func<StudentEntry,bool>> expression);
        IEnumerable<StudentEntry> GetStudents(Expression<Func<StudentEntry, bool>> expression);
        IEnumerable<StudentEntry> GetAllStudent();

        Task<int> SaveChangesAsync();
    }
}
