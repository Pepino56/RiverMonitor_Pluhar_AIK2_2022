using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RiverMonitor.Data
{
    public record Station
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(30), Display(Name = "Title")]
        public string Title { get; set; }
        [Required, Range(0, 100), Display(Name = "Flood warning value")]
        public int FloodWarningValue { get; set; }
        [Required, Range(0, 100), Display(Name = "Drought warni value")]
        public int DroughtWarniValue { get; set; }

        [JsonIgnore]
        public ICollection<Value> Values { get; set; }
    }
}


