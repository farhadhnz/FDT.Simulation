using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace FDT.Simulation.API.DTOs
{
    public class DigitalTwinDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonProperty("dateModified")]
        public DateTime DateModified { get; set; }
        [JsonProperty("projectId")]
        public int ProjectId { get; set; }
        [JsonProperty("digitalTwinTypeId")]
        public int DigitalTwinTypeId { get; set; }
        [JsonProperty("properties")]
        public virtual ICollection<DigitalTwinPropertyDto>? Properties { get; set; }
    }

    
}
