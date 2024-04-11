using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace furni.Areas.Admin.Models
{
    public class CouponModel : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã không được vượt quá 50 ký tự")]
        [Display(Name = "Mã")]
        public string Code { get; set; } // Unique identifier for the coupon.

        [Required(ErrorMessage = "Phần trăm giảm giá là bắt buộc")]
        [Range(0, 70, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 70")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Phần trăm giảm giá")]
        public decimal DiscountPercentage { get; set; } // Percentage discount the coupon provides.

        [DataType(DataType.Date)]
        [Display(Name = "Hiệu lực từ")]
        public DateTime ValidFrom { get; set; } // Start date from when the coupon can be used.

        [DataType(DataType.Date)]
        [Display(Name = "Hiệu lực đến")]
        public DateTime ValidUntil { get; set; } // Expiry date after which the coupon cannot be used.

        [Display(Name = "Kích hoạt")]
        [Required]
        public bool IsActive { get; set; } = true; // Indicates whether the coupon is currently usable.

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Số tiền đơn hàng tối thiểu")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền đơn hàng tối thiểu phải lớn hơn hoặc bằng 0")]
        public decimal? MinimumOrderAmount { get; set; }

        // Custom validation logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ValidFrom >= ValidUntil)
            {
                yield return new ValidationResult(
                    "Ngày hết hiệu lực phải lớn hơn ngày có hiệu lực.",
                    new[] { nameof(ValidFrom), nameof(ValidUntil) });
            }

            if (ValidUntil <= DateTime.Today)
            {
                yield return new ValidationResult(
                    "Ngày hết hiệu lực phải sau ngày hiện tại.",
                    new[] { nameof(ValidUntil) });
            }

            if (ValidFrom <= DateTime.Today)
            {
                yield return new ValidationResult(
                    "Ngày có hiệu lực phải sau ngày hiện tại.",
                    new[] { nameof(ValidUntil) });
            }
        }
    }
}
