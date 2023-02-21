using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("BeneficiariesFamiliy")]
    public class BeneficiariesFamilyEntity: GenericEntity
    {
        [Required]
        [ForeignKey("DocumentTypeId")]
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public virtual DocumentTypeEntity DocumentType { get; set; } = new();
        [Required]
        public string DocumentNumber { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        [Required]
        [ForeignKey("FamilyRelationId")]
        public Guid FamilyRelationId { get; set; }
        public virtual BeneficiariesParametersEntity FamilyRelation { get; set; } = new();
        public bool Attendant { get; set; } = false;
    }
}
