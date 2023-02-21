using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DevelopmentRoomGroupByYear")]
    public class DevelopmentRoomGroupByYearEntity : GenericEntity
    {
        [Required]
        [ForeignKey("TrainingCenterId")]
        public Guid TrainingCenterId { get; set; }
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();
        [Required]
        [ForeignKey("CampusId")]
        public Guid CampusId { get; set; }
        public virtual CampusEntity Campus { get; set; } = new();
        [Required]
        [ForeignKey("DevelopmentRoomId")]
        public Guid DevelopmentRoomId { get; set; }
        public virtual DevelopmentRoomEntity DevelopmentRoom { get; set; } = new();
        public int Year { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
