namespace school_rest_api
{
    public static class Constants
    {
        public static char[] RangeOfClassNames = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };

        public const string GetAllClassesQueryKey = "get_all_classes_query";
        public const string GetAllEducatorsQueryKey = "get_all_educators_query";
        public const string GetAllStudentsQueryKey = "get_all_students_query";
        public const string GetClassByIdQueryFormatKey = "get_class_by_id_query_{0}";
        public const string GetEducatorByIdQueryFormatKey = "get_educator_by_id_query_{0}";
        public const string GetStudentByIdQueryFormatKey = "get_student_by_id_query_{0}";
        public const string GetStudentsByClassAndGroupQueryFormatKey = "get_students_by_class_and_group_query_{0}_{1}";
        public const string GetStudentsByClassAndSortedByGenderQueryFormatKey = "get_students_by_class_and_sorted_by_gender_query_{0}_{1}";
    }
}
