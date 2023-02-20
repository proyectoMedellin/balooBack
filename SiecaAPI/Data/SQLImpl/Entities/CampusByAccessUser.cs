using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("CampusByAccessUser")]
    public class CampusByAccessUserEntity
    {
        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; }
        [Required]
        [ForeignKey("TrainingCenterId")]
        public Guid TrainingCenterId { get; set; }
        [Required]
        [ForeignKey("CampusId")]
        public Guid CampusId { get; set; }
        [Required]
        [ForeignKey("AccessUserId")]
        public Guid AccessUserId { get; set; }

        public CampusByAccessUserEntity(Guid organizationid, Guid trainingCenterId, Guid campusId, Guid accesUserId)
        {
            OrganizationId = organizationid;
            TrainingCenterId = trainingCenterId;
            CampusId = campusId;
            AccessUserId = accesUserId;
        }
        public CampusByAccessUserEntity()
        {
            OrganizationId = Guid.Empty;
            TrainingCenterId = Guid.Empty;
            CampusId = Guid.Empty;
            AccessUserId = Guid.Empty;
        }
    }
}
