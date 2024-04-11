using System.ComponentModel.DataAnnotations;

namespace furni.Areas.Admin.Models
{
    public class UserModel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        [Display(Name = "Họ và chữ lót")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "User Name cannot be longer than 50 characters.")]
        [Display(Name = "Tên người dùng")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
    }
}
