namespace WebMvcProject.Data
{
    public class TeacherCourse
    {
        public int TeacherCourseId { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
    }
}
