using Newtonsoft.Json;

namespace FDT.Simulation.API.DTOs
{
    public class DigitalTwinPropertyDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }
        [JsonProperty("propertyValue")]
        public string PropertyValue { get; set; }
        [JsonProperty("digitalTwinId")]
        public int DigitalTwinId { get; set; }
        [JsonIgnore] // Optional: Ignore this property during deserialization
        public DigitalTwinDto DigitalTwin { get; set; }
    }
}
