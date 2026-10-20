using System.ComponentModel.DataAnnotations;

namespace Project.Common.Models.ViewModels.Auth
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Validation_Id_Required")]
        [Display(Name = "Id_DisplayName")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Validation_Password_Required")]
        [Display(Name = "Password_DisplayName")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string SystemMessage { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
