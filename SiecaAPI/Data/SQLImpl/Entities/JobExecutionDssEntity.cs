using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("JobExecutionDss")]
    public class JobExecutionDssEntity : GenericEntity
    {
        public DateTime FechaEjecucion { get; set; } = DateTime.UtcNow;
        public int CantidadRegistros { get; set; } = 0;
        public string BeneficiaryId { get; set; } = string.Empty;
    }
}
