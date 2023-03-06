using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("BeneficiaryAnthropometricData")]
    public class BeneficiaryAnthropometricRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public virtual BeneficiariesEntity Beneficiary { get; set; } = new();
        [Required]
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();
        public string? Comment { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Height { get; set; }
        [Required]
        public decimal Bmi { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
