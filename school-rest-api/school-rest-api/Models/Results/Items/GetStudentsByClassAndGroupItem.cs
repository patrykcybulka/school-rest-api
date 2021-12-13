using school_rest_api.Enums;

namespace school_rest_api.Models.Results.Items
{
    public class GetStudentsByClassAndGroupItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EGender Gender { get; set; }
    }
}
