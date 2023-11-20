using System.ComponentModel.DataAnnotations;

namespace WebMvcProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name can be max 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname can be max 30 characters")]
        public string SurName { get; set; }

        //[Display(Name="Kullanıcı Adı", Prompt ="cisema." )]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username can be max 50 characters")]
        public string UserName { get; set; }

        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password can be min 6 characters")]
        [MaxLength(12, ErrorMessage = "Password can be max 12 characters")]
        public string Password { get; set; }


        [Required(ErrorMessage = "EmailAddress is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Re-Password is required")]
        [MinLength(6, ErrorMessage = "Re-Password can be min 6 characters")]
        [MaxLength(12, ErrorMessage = "Re-Password can be max 12 characters")]
        [Compare(nameof(Password))] //karşılaştır
        public string RePassword { get; set; }
    }
}
