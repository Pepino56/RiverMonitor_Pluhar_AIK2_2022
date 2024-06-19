namespace SimulatorOfValues.Contracts
{
    public class FloodReport
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int StationId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Val { get; set; }
    }
}