using System.ComponentModel.DataAnnotations;

namespace RiverMonitor.Data
{
    public class Value
    {
        [Key]
        public Guid Id { get; set; }
        public int Val { get; set; }
        public DateTime TimeStamp { get; set; }
        public int StationId { get; set; }
        public Station? Station { get; set; }
    }
}
