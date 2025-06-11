using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GraduProjj.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Column("first_name", TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("last_name", TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? ProfilePicturePath { get; set; } // Nullable profile picture path

        public string? EmailVerificationCode { get; set; }  // الكود المؤقت
        public bool IsEmailVerified { get; set; } = false;  // تم التحقق من الإيميل؟



        // OTP Code and Expiration
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiration { get; set; }


        // علاقات التنقل
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        //public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
        public virtual ICollection<Input> Input { get; set; } = new List<Input>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
