using System.ComponentModel.DataAnnotations;
using WebMvcProject.Data;

namespace WebMvcProject.Models
{
    public class CourseStudentViewModel
    {
        [Required(ErrorMessage = "Kurs Seçimi Yapınız")]
        public int? CourseId { get; set; }
        [Required(ErrorMessage = "Öğrenci Seçimi Yapınız")]
        public int? StudentId { get; set; }
    }
}