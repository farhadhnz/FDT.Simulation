namespace FDT.Simulation.API.DTOs
{
    public class DataReceivedDto
    {
        public int DigitalTwinId { get; set; }
        public DateTime ReceivedTime { get; set; }
        public double DataValue { get; set; }
        public string Event { get; set; }
    }
}
